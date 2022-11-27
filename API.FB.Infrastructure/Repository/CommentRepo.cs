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
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_postID", comment.PostID);
                parameters.Add("@v_token", comment.Token);
                parameters.Add("@v_index", comment.index);
                parameters.Add("@v_count", comment.count ?? 10);

                var data = _dbConnection.Query<Comment>($"Proc_GetComment", param: parameters, commandType: CommandType.StoredProcedure).ToList();

                return data;
            }
        }

        public List<Comment> InsertComment(Comment comment)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_postID", comment.PostID);
                parameters.Add("@v_token", comment.Token);
                parameters.Add("@v_comment", comment.content);
                parameters.Add("@v_index", comment.index ?? 1);
                parameters.Add("@v_count", comment.count ?? 10);

                var data = _dbConnection.Query<Comment>($"Proc_InsertComment", param: parameters, commandType: CommandType.StoredProcedure).ToList();

                return data;
            }
        }
    }
}
