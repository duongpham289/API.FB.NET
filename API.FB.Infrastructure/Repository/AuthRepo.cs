using API.FB.Core.Entities;
using API.FB.Core.Interfaces.Repository;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Infrastructure.Repository
{
    public class AuthRepo : BaseRepo<User>, IAuthRepo
    {

        public AuthRepo(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Kiểm tra xem người dùng đã tồn tại hay chưa thông qua số điẹn thoại
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// lttuan1
        public User CheckSignupLegal(User user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@phonenumber", user.PhoneNumber);
            parameters.Add($"@password", user.Password);
            var res = _sqlConnection.QueryFirstOrDefault($"Proc_Getuser", param: parameters, commandType: CommandType.StoredProcedure);
            return res;
        }

        /// <summary>
        /// Kiểm tra thông tin đăng nhập có dúng k 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        /// lttuan
        public User getUserByEmail(Auth login)
        {
            var email = login.Email;
            var password = login.PassWord;
            var sqlQuerry = $"SELECT * FROM user WHERE Email = @email AND Password = @password";
            var parameters = new DynamicParameters();
            parameters.Add("email", email);
            parameters.Add("password", password);
            var res = _sqlConnection.QueryFirstOrDefault<User>(sqlQuerry, param: parameters);
            return res;
        }
    }
}
