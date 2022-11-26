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
        [HttpPost("sign_up")]
        public ServiceResult Signup([FromForm] User user)
        {
            ServiceResult result = new ServiceResult();
            try
            {

                if (String.IsNullOrWhiteSpace(user.PhoneNumber))
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                // Kiểm tra đữ liệu
                var isIllegal = _authRepo.CheckSignupLegal(user);

                //Thực hiện validate dữ liệu
                if (isIllegal)
                {
                    if (String.IsNullOrWhiteSpace(user.Password))
                    {
                        result.ResponseCode = 1002;
                        result.Message = "Số lượng Parameter không đầy đủ";
                        return result;
                    }
                    else
                    {
                        result.ResponseCode = 9996;
                        result.Message = "Người dùng đã tồn tại";
                        return result;

                    }
                }
                else
                {
                    // Thêm mới người dùng
                    var userID = _userRepository.Insert(user);

                    // Trả về result
                    result.Data = new
                    {
                        UserID = userID,
                        UserName = user.FullName,
                    };

                    result.ResponseCode = 1000;
                    result.Message = "OK";
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
        [HttpPost("log_in")]
        public async Task<ServiceResult> Login([FromForm] Auth auth)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if (String.IsNullOrWhiteSpace(auth.Password) || String.IsNullOrWhiteSpace(auth.PhoneNumber))
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                // Kiểm tra user
                var user = AuthenticateUser(auth);

                if (user == null)
                {
                    result.ResponseCode = 9995;
                    result.Message = "Không có người dùng này";
                    return result;
                }
                else if (!String.IsNullOrWhiteSpace(user.Token))
                {
                    result.ResponseCode = 1010;
                    result.Message = "Hành động đã được người dùng thực hiện trước đây";
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

                result.Data = new { user.UserID, user.FullName, Token = tokenString, user.Avatar };
                result.ResponseCode = 1000;
                result.Message = "OK";
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
        [HttpGet("log_out")]
        public async Task<ServiceResult> LogOut([FromForm] string token)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if (token == null)
                {

                    result.ResponseCode = 9998;
                    result.Message = "Sai token";
                    return result;
                }

                // Goi user theo token
                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }
                else if (user.Token != token)
                {

                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }

                user.Token = null;

                // Update token cho user
                _userRepository.UpdateTokenForUser(user);
                result.ResponseCode = 1000;
                result.Message = "OK";
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
