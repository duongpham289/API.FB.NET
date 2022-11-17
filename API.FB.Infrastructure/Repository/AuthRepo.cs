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
        public bool CheckSignupLegal(User user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@v_phoneNumber", user.PhoneNumber);
            parameters.Add("@v_isIllegal", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            _sqlConnection.QueryFirstOrDefault($"Proc_CheckSignupLegal", param: parameters, commandType: CommandType.StoredProcedure);

            var isIllegal = parameters.Get<Boolean>("@v_isIllegal");
            return isIllegal;
        }

        /// <summary>
        /// Kiểm tra thông tin đăng nhập có dúng k 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        /// lttuan
        public User getUserByPhoneNumber(Auth login)
        {
            var parameters = new DynamicParameters();
            parameters.Add("v_phoneNumber", login.PhoneNumber);
            parameters.Add("v_password", login.Password);
            var user = _sqlConnection.QueryFirstOrDefault<User>($"Proc_CheckLoginInfo", param: parameters, commandType: CommandType.StoredProcedure);

            return user;
        }
    }
}
