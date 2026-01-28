// Services/TokenService.cs
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentalManager.Data;
using RentalManager.DTOs.Authentication;
using RentalManager.Models;
using RentalManager.Repositories.TokenRepository;
using RentalManager.Services.TokenService;
using ServiceStack.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenRepository _tokenrepo;
    private readonly RSA _privateRsa;
    private readonly RsaSecurityKey _privateKey;
    private readonly RsaSecurityKey _publicKey;

    // Access token lifetime
    private readonly TimeSpan _accessTokenLifetime = TimeSpan.FromMinutes(15);
    private readonly TimeSpan _refreshTokenLifetime = TimeSpan.FromDays(30);

    // constructor
    public TokenService(    
        IWebHostEnvironment env,
        IConfiguration config,
        UserManager<ApplicationUser> userManager,
        ITokenRepository tokenrepo)
    {
        _config = config;
        _userManager = userManager;
        _tokenrepo = tokenrepo;

        var privateKeyPath = Path.Combine(env.ContentRootPath, "Keys", "private.pem");
        if (!File.Exists(privateKeyPath))
            throw new FileNotFoundException("Private key file not found", privateKeyPath);


        // PRIVATE KEY (signing)
        var privatePem = File.ReadAllText(privateKeyPath);
        _privateRsa = RSA.Create();
        _privateRsa.ImportFromPem(privatePem);
        _privateKey = new RsaSecurityKey(_privateRsa);
    }




    public async Task<AuthResultDto> GenerateTokensAsync(ApplicationUser user, string ipAddress)
    {
        try
        {
            var accessToken = await CreateAccessTokenAsync(user);

            // create refresh token
            var refreshTokenValue = GenerateRandomToken();
            var refresh = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                ExpiresOn = DateTimeOffset.UtcNow.Add(_refreshTokenLifetime),
                CreatedOn = DateTimeOffset.UtcNow,
                CreatedByIp = ipAddress,
                Revoked = false
            };

            await _tokenrepo.AddRefreshTokenAsync(refresh);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);

            return new AuthResultDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                RefreshTokenExpiry = refresh.ExpiresOn
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.InnerException?.Message ?? ex.Message);
        }
    }



    public async Task<string> CreateAccessTokenAsync(ApplicationUser user)
    {
        var now = DateTime.UtcNow;
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "";


        // build claims
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Name, user.FirstName ?? string.Empty),
            new Claim("accountId", user.AccountId?.ToString() ?? string.Empty),
            new Claim("roles", role)
        };


        var creds = new SigningCredentials(_privateKey, SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            notBefore: now,
            expires: now.Add(_accessTokenLifetime),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }



    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var parameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = _config["Jwt:Audience"],
            ValidateIssuer = true,
            ValidIssuer = _config["Jwt:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _publicKey,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };

        try
        {
            return tokenHandler.ValidateToken(token, parameters, out var validatedToken);
        }
        catch
        {
            return null;
        }
    }



    private static string GenerateRandomToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }



    public async Task<ApiResponse<AuthResultDto>> RefreshTokenAsync(string refreshToken, string ip)
    {

        try
        {
            var token = await _tokenrepo.FindRefreshToken(refreshToken);

            if (token == null)
                return new ApiResponse<AuthResultDto>(null, "Invalid Token", false);

            // DETECT REUSE
            if (token.Revoked)
            {
                // TOKEN REUSE ATTACK
                await _tokenrepo.RevokeAllTokensAsync(token.UserId, ip);

                return new ApiResponse<AuthResultDto>(
                    null,
                    "Security violation detected. Please login again.",
                    false
                );
            }

            // Expiry check
            if (token.ExpiresOn <= DateTimeOffset.UtcNow)
                return new ApiResponse<AuthResultDto>(null, "Token expired", false);



            // Generate New Token
            var newTokens = await GenerateTokensAsync(token.User, ip);

            if(newTokens == null)
                return new ApiResponse<AuthResultDto>(null, "Failed To Generate Token", false);


            // Revoke old token
            token.Revoked = true;
            token.RevokedOn = DateTimeOffset.UtcNow;
            token.RevokedByIp = ip;
            token.ReplacedByToken = newTokens.RefreshToken;
            await _tokenrepo.UpdateRefreshTokenAsync(token);

            return new ApiResponse<AuthResultDto>(newTokens, "Token Refreshed");
        }
        catch(Exception ex)
        {
            return new ApiResponse<AuthResultDto>(null, $"Error Occurred: {ex.InnerException?.Message ?? ex.Message}", false);
        }
    }


}
