//using API.FB.Core.Entities;
//using API.FB.Core.Interfaces.Repository;
//using CNWTTBL.Entities;
//using Dapper;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CNWTTDL.Repository
//{
//    public class AuthRepo : BaseRepo<User>, IAuthRepo
//    {
//        public AuthRepo(IConfiguration configuration) : base(configuration)
//        {
//        }

//        public User getUserByEmail(Auth login)
//        {
//            var email = login.Email;
//            var password = login.PassWord;
//            var sqlQuerry = $"SELECT * FROM user WHERE Email = @email AND Password = @password";
//            var parameters = new DynamicParameters();
//            parameters.Add("email", email);
//            parameters.Add("password", password);
//            var res = _sqlConnection.QueryFirstOrDefault<User>(sqlQuerry, param: parameters);
//            return res;
//        }
//    }
//}
