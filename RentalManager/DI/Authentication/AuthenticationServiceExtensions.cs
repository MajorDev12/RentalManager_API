using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using RentalManager.Services.TokenService;

namespace RentalManager.DI.Authentication
{
    public static class AuthenticationServiceExtensions
    {
        public static IServiceCollection AddAuthenticationServices(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            // Load RSA public key
            var publicKeyPath = Path.Combine(environment.ContentRootPath, "Keys", "public.pem");

            if (!File.Exists(publicKeyPath))
                throw new Exception("Public key file not found");

            var publicPem = File.ReadAllText(publicKeyPath);

            var rsa = RSA.Create();
            rsa.ImportFromPem(publicPem.ToCharArray());

            var publicKey = new RsaSecurityKey(rsa)
            {
                KeyId = "rsa-key-2025-01"
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = publicKey,

                    RequireSignedTokens = true,
                    RequireExpirationTime = true
                };
            });

            return services;
        }
    }
}