using API.FB.Core.Entities;
using CNWTTBL.Entities;
using CNWTTBL.Interfaces.Repositories;
using CNWTTBL.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MISA.Core.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CNWTT.Controllers
{
    [Route("api/fb/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IConfiguration _configuration;
        IAuthRepo _authRepo;
        IAuthService _authService;

        public AuthController(IAuthRepo authRepo, IAuthService authService, IConfiguration configuration)
        {
            _authRepo = authRepo;
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public IActionResult Post([FromBody] User entity)
        {
            try
            {
                var res = _authService.InsertService(entity);
                return StatusCode(201, res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Xử lí sự kiện login
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        //POST api/<AuthController>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Auth auth)
        {
            try
            {
                var user = AuthenticateUser(auth);

                if (user == null)
                {
                    return BadRequest();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, "Manager"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var tokenString = this.GenerateAccessToken(claims);

                var tokenBearerString = "Bearer " + tokenString;

                var refreshToken = this.GenerateRefreshToken();

                return Ok(new { user, Token = tokenString, TokenBearer = tokenBearerString, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Sinh token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken
                    (
                        issuer: _configuration["Url"],
                        audience: _configuration["Url"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: signingCredentials
                    );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }

        /// <summary>
        /// RefreshToken
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            var refreshToken = this.GenerateRefreshToken();

            var refreshTokenBearer = "Bearer " + refreshToken;
            //Redirect to home page    
            return Ok(new { RefreshToken = refreshToken, RefreshTokenBearer = refreshTokenBearer });
        }

        /// <summary>
        /// Kiểm tra email và password có hợp lệ khong
        /// </summary>
        /// <returns> Nếu hợp lệ - Thông tin user đã đang nhập, Nếu không hợp lệ - trả về null  </returns>
        private User AuthenticateUser(Auth auth)
        {
            User res = _authRepo.getUserByEmail(auth);
            if (res != null)
            {
                return res;
            }
            else
            {
                return null;
            }
        }
        protected IActionResult HandleException(Exception ex)
        {
            var res = new
            {
                devMsg = ex.Message,
                userMsg = "Có lỗi xấy ra vui lòng liên hệ  để được hỗ trợ",
                errorCode = "001",
                data = ex.Data
            };
            if (ex is HUSTValidateException)
                return StatusCode(200, res);
            else
                return StatusCode(500, res); //Lỗi từ server trả về 500
        }
    }
}
