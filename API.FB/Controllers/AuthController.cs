using API.FB.Core.Entities;
using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
//using Microsoft.IdentityModel.Tokens;
//using MISA.Core.Exceptions;
//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.fb.Controllers
{
    [Route("fb")]
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
        public ServiceResult Post([FromBody] User entity)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                result.Data = _authService.InsertService(entity);
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;
        }

        /// <summary>
        /// Xử lí sự kiện login
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        //POST api/<AuthController>
        [HttpPost("login")]
        public async Task<ServiceResult> Login([FromBody] Auth auth)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var user = AuthenticateUser(auth);

                if (user == null)
                {
                    result.ResponseCode = 9995;
                    result.Message = "Không có người dùng này";
                    return result;
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.PhoneNumber),
                    new Claim(ClaimTypes.Role, "Manager"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var tokenString = this.GenerateAccessToken(claims);

                var tokenBearerString = "Bearer " + tokenString;

                var refreshToken = this.GenerateRefreshToken();

                result.Data = new { user.UserID, user.FullName, Token = tokenBearerString, user.Avatar };
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;
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
    }
}
