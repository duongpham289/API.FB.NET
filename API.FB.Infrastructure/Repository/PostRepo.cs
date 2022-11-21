using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Entities;
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
    public class PostRepo : BaseRepo<Post>, IPostRepo
    {
        protected IConfiguration _configuration;
        protected IDbConnection _dbConnection;
        string _className;

        #region Constructor
        public PostRepo(IConfiguration configuration) : base(configuration)
        {

            _configuration = configuration;

        }
        #endregion

        /// <summary>
        /// Lấy dữ liệu Mã entity
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        public List<Post> GetListPost(Guid userID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@v_userID", userID);
            var listPost = _sqlConnection.Query<Post>($"Proc_GetListPost", param: parameters, commandType: CommandType.StoredProcedure);

            return listPost.AsList();
        }

        /// <summary>
        /// Lấy dữ liệu Mã entity
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        public List<Post> GetNewListPost(Guid userID, int newestPostID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@v_userID", userID);
            var listPost = _sqlConnection.Query<Post>($"Proc_GetListPost", param: parameters, commandType: CommandType.StoredProcedure);

            var listPostData = listPost.AsList();
            var newestPost = new Post();

            if (listPost != null && listPost.Count() > 0)
            {
                return listPost.AsList();
            }

            return null;

        }


        public int DeleteCustom(Guid userID, Guid postID)
        {  // Khỏi tạo câu truy vấn:
            var sqlQuery = $"DELETE FROM like_post WHERE UserID = @userID and PostID = @postID";
            var parameters = new DynamicParameters();
            parameters.Add("@userID", userID);
            parameters.Add("@postID", postID);
            // Thực hiện câu truy vấn: 
            var res = _sqlConnection.Execute(sqlQuery, param: parameters);
            return res;
        }

        public int UpdatePost(Post post)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@v_described", post.Described);
                parameters.Add("@v_token", post.Token);
                parameters.Add("@v_postID", post.PostID);
                parameters.Add("@v_media", post.Media);
                parameters.Add("@v_status", post.Status);

                var data = _dbConnection.Execute($"Proc_UpdatePost", param: parameters, commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        public int InsertPost(Post post)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@v_described", post.Described);
                parameters.Add("@v_token", post.Token);
                parameters.Add("@v_media", post.Media);
                parameters.Add("@v_status", post.Status);

                var data = _dbConnection.Execute($"Proc_InsertPost", param: parameters, commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        public int DeletePost(Post post)
        {

            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_postID", post.PostID);


                var data = _dbConnection.Execute($"Proc_DeletePost", param: parameters, commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        public int ReportPost(Report report)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_postID", report.PostID);
                parameters.Add("@v_token", report.Token);
                parameters.Add("@v_subject", report.Subject);
                parameters.Add("@v_details", report.Details);

                var data = _dbConnection.Execute($"Proc_ReportPost", param: report, commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        public int ReactPost(React react)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@v_token", react.Token);
                parameters.Add("@v_postID", react.PostID);

                var data = _dbConnection.Execute($"Proc_ReactPost", param: react, commandType: CommandType.StoredProcedure);

                return data;
            }
        }

    }
}
