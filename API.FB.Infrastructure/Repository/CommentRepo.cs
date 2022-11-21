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
            _configuration = configuration;
        }

        public List<Comment> GetByPostId(Comment comment)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {

                var token = comment.Token;
                var postId = comment.PostID;
                var index = comment.Index;
                var count = comment.Count;

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_postID", comment.PostID);
                parameters.Add("@v_token", comment.Token);
                parameters.Add("@v_index", comment.Index);
                parameters.Add("@v_count", comment.Count);

                var data = _dbConnection.Query<List<Comment>>($"Proc_GetComment", param: parameters, commandType: CommandType.StoredProcedure);

                return (List<Comment>)data; // làm tạm, chưa đúng
            }
        }

        public int InsertComment(Comment comment)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_postID", comment.PostID);
                parameters.Add("@v_token", comment.Token);
                parameters.Add("@v_comment", comment.CommentContent);
                parameters.Add("@v_index", comment.Index);
                parameters.Add("@v_count", comment.Count);

                var data = _dbConnection.Execute($"Proc_InsertComment", param: comment, commandType: CommandType.StoredProcedure);

                return data;
            }
        }
    }
}
