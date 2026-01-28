using Microsoft.AspNetCore.Mvc;
using RentalManager.DTOs.Authentication;
using RentalManager.Helpers.Authorization;
using RentalManager.Services.AuthService;
using RentalManager.Services.TokenService;

namespace RentalManager.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly ITokenService _tokenservice;
        public AuthenticationController(IAuthService service, ITokenService tokenservice)
        {
            _service = service;
            _tokenservice = tokenservice;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var result = await _service.Register(dto);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                var result = await _service.Login(dto, ip);

                if (result.Success == false || result.Data == null) return BadRequest(result);


                SetRefreshCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpiry);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                var userId = User.UserId();

                var result = await _service.Logout(userId, ip);

                if (result.Success == false) return BadRequest(result);

                Response.Cookies.Delete("refreshToken");

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            try 
            {
                var refreshToken = Request.Cookies["refreshToken"];
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";


                if (string.IsNullOrEmpty(refreshToken))
                    return Unauthorized();

                var result = await _tokenservice.RefreshTokenAsync(refreshToken, ip);

                if (!result.Success || result.Data == null)
                    return Unauthorized(result);

                
                SetRefreshCookie(
                    result.Data.RefreshToken,
                    result.Data.RefreshTokenExpiry
                );

                return Ok(result);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




        private void SetRefreshCookie(string refreshToken, DateTimeOffset expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expires,
                Path = "/api"
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }






    }
}
