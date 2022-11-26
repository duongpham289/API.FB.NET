using API.FB.Core.FBAttribute;
using API.FB.Core.Interfaces.Repository;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Infrastructure.Repository
{
    public class BaseRepo<T> : IBaseRepo<T>
    {
        /// <summary>
        /// Xử lí kết nối với database 
        /// </summary>
        IConfiguration _configuration;
        readonly string _connectionString = string.Empty;
        protected MySqlConnection _sqlConnection;
        string _tableName;
        public BaseRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            // Khai báo thông tin database:
            _connectionString = _configuration.GetConnectionString("SqlConnection");
            // Khỏi tạo kết nối đến database --> sử dụng mySqlConnector
            _sqlConnection = new MySqlConnection(_connectionString);
            _tableName = typeof(T).Name.ToLower();
        }
    }
}
