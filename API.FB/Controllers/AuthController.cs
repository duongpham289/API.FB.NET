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
using System.Text.RegularExpressions;
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
        IUserRepository _userRepository;

        public AuthController(IAuthRepo authRepo, IConfiguration configuration, IUserRepository userRepository)
        {
            _authRepo = authRepo;
            _configuration = configuration;
            _userRepository = userRepository;
        }


        /// <summary>
        /// Đăng kí
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// lttuan1
        [HttpPost("signup")]
        public ServiceResult Signup([FromForm] User user)
        {
            ServiceResult result = new ServiceResult();
            try
            {

                if (String.IsNullOrWhiteSpace(user.PhoneNumber))
                {
                    result.code = "1002";
                    result.message = "Parameter is not enough";
                    return result;
                }

                // Kiểm tra đữ liệu
                var isIllegal = _authRepo.CheckSignupLegal(user);

                //Thực hiện validate dữ liệu
                if (isIllegal)
                {
                    if (String.IsNullOrWhiteSpace(user.Password))
                    {
                        result.code = "1002";
                        result.message = "Parameter is not enough";
                        return result;
                    }
                    else
                    {
                        result.code = "9996";
                        result.message = "User existed";
                        return result;

                    }
                }
                else
                {
                    if (user.Password == user.PhoneNumber || !Regex.Match(user.PhoneNumber, @"\b(0[3|5|7|8|9])+([0-9]{8})\b").Success)
                    {
                        result.code = "1004";
                        result.message = "Paramerter value is invalid";
                        return result;
                    }

                    // Thêm mới người dùng
                    var userID = _userRepository.Insert(user);

                    // Trả về result
                    result.data = new
                    {
                        id = userID,
                        username = user.FullName,
                    };

                    result.code = "1000";
                    result.message = "OK";
                    return result;
                }
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
        public async Task<ServiceResult> Login([FromForm] Auth auth)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if (String.IsNullOrWhiteSpace(auth.Password) || String.IsNullOrWhiteSpace(auth.PhoneNumber))
                {
                    result.code = "1002";
                    result.message = "Parameter is not enough";
                    return result;
                }

                if (auth.Password == auth.PhoneNumber || !Regex.Match(auth.PhoneNumber, @"\b(0[3|5|7|8|9])+([0-9]{8})\b").Success)
                {
                    result.code = "1004";
                    result.message = "Paramerter value is invalid";
                    return result;
                }
                // Kiểm tra user
                var user = AuthenticateUser(auth);

                if (user == null)
                {
                    result.code = "9995";
                    result.message = "User is not validated";
                    return result;
                }
                else if (!String.IsNullOrWhiteSpace(user.Token))
                {
                    result.code = "1010";
                    result.message = "Action has been done previously by this user";
                    return result;

                }

                // Tạo token
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.PhoneNumber),
                    new Claim(ClaimTypes.Role, "Manager"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var tokenString = this.GenerateAccessToken(claims);
                user.Token = tokenString;
                // Update token cho user
                _userRepository.UpdateTokenForUser(user);

                result.data = new { 
                    id = user.UserID, 
                    username = user.FullName, 
                    token = tokenString, 
                    avatar = user.Avatar };
                result.code = "1000";
                result.message = "OK";
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
                        //expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: signingCredentials
                    );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }

        /// <summary>
        /// RefreshToken
        /// </summary>
        /// <returns></returns>
        //private string GenerateRefreshToken()
        //{
        //    var randomNumber = new byte[32];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(randomNumber);
        //        return Convert.ToBase64String(randomNumber);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        public async Task<ServiceResult> LogOut([FromForm] string token)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if (token == null)
                {

                    result.code = "9998";
                    result.message = "Token is invalid";
                    return result;
                }

                // Goi user theo token
                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }
                else if (user.Token != token)
                {

                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }

                user.Token = null;

                // Update token cho user
                _userRepository.UpdateTokenForUser(user);
                result.code = "1000";
                result.message = "OK";
                return result;
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;
        }

        /// <summary>
        /// Kiểm tra email và password có hợp lệ khong
        /// </summary>
        /// <returns> Nếu hợp lệ - Thông tin user đã đang nhập, Nếu không hợp lệ - trả về null  </returns>
        private User AuthenticateUser(Auth auth)
        {
            User res = _authRepo.getUserByPhoneNumber(auth);
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
