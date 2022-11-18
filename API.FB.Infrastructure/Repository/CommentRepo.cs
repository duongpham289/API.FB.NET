using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data;

namespace API.FB.Infrastructure.Repository
{
    public class CommentRepo : BaseRepo<Comment>, ICommentRepo
    {
        protected IConfiguration _configuration;
        protected IDbConnection _dbConnection;
        public CommentRepo(IConfiguration configuration) : base(configuration)
        {
        }

        public List<Comment> GetByPostId(Guid postId)
        {
            // Thực hiện khai báo câu lệnh truy vấn SQL:
            var sqlQuery = $"SELECT * FROM comment c WHERE c.PostID = @postId";
            var parameters = new DynamicParameters();
            parameters.Add("@postId", postId);

            // Thực hiện câu truy vấn:
            var res = _sqlConnection.Query<Comment>(sqlQuery, param: parameters);

            // Trả về dữ liệu dạng List:
            return res.ToList();
        }

        public int EditComment(Comment comment)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                var data = _dbConnection.Execute($"Proc_UpdateComment", param: comment, commandType: CommandType.StoredProcedure);

                return data;
            }
        }
    }
}
