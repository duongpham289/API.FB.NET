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
        /// Hàm trả về danh sách bài viết
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userID"></param>
        /// <param name="lastedPostID"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<PostCustom> GetListPost(string token, Guid userID, Guid lastedPostID, int skip, int take)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_postID", lastedPostID);
                parameters.Add("@v_userID", userID);
                parameters.Add("@v_token", token);
                parameters.Add("@v_skip", skip);
                parameters.Add("@v_take", take);
                var data = _dbConnection.Query<PostCustom>($"Proc_ReactPost", param: parameters, commandType: CommandType.StoredProcedure);

                return data.ToList();
            }
        }

        /// <summary>
        /// Lấy dữ liệu Mã entity
        /// </summary>
        /// <returns></returns>
        public List<PostCustom> GetNewListPost(string token, Guid lastedPostID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@v_postID", lastedPostID);
            parameters.Add("@v_token", token);
            var data = _dbConnection.Query<PostCustom>($"Proc_GetNewPost", param: parameters, commandType: CommandType.StoredProcedure);

            return data.ToList();

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
                var data = _dbConnection.Execute($"Proc_UpdatePost", param: post, commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        public int InsertPost(Post post)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                var data = _dbConnection.Execute($"Proc_InsertPost", param: post, commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        public int DeletePost(Guid postID)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_postID", postID);


                var data = _dbConnection.Execute($"Proc_DeletePost", param: parameters, commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        public int ReportPost(Report report)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                var data = _dbConnection.Execute($"Proc_ReportPost", param: report, commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        /// <summary>
        /// Hàm like post 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="postID"></param>
        /// <returns></returns>
        public int LikePost(string token, Guid postID)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_postID", postID);
                parameters.Add("@v_token", token);
                var data = _dbConnection.Execute($"Proc_ReactPost", param: parameters, commandType: CommandType.StoredProcedure);

                return data;
            }
        }
    }
}
