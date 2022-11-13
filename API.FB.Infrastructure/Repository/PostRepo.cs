//using API.FB.Core.Interfaces.Repository;
//using CNWTTBL.Entities;
//using Dapper;
//using Microsoft.Extensions.Configuration;
//using MySqlConnector;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CNWTTDL.Repository
//{
//    public class PostRepo 
//    {


//        #region Constructor
//        public PostRepo(IConfiguration configuration) 
//        {

//        }
//        #endregion

//        /// <summary>
//        /// Lấy dữ liệu Mã entity
//        /// </summary>
//        /// <returns></returns>
//        /// CreatedBy: PHDUONG(27/08/2021)
//        public List<Post> GetListPost(Guid userID)
//        {
//            DynamicParameters parameters = new DynamicParameters();
//            parameters.Add($"@v_userID", userID);
//            var listPost = _sqlConnection.Query<Post>($"Proc_GetListPost", param: parameters, commandType: CommandType.StoredProcedure);

//            return listPost.AsList();
//        }

//        /// <summary>
//        /// Lấy dữ liệu Mã entity
//        /// </summary>
//        /// <returns></returns>
//        /// CreatedBy: PHDUONG(27/08/2021)
//        public List<Post> GetNewListPost(Guid userID, int newestPostID)
//        {
//            DynamicParameters parameters = new DynamicParameters();
//            parameters.Add($"@v_userID", userID);
//            var listPost = _sqlConnection.Query<Post>($"Proc_GetListPost", param: parameters, commandType: CommandType.StoredProcedure);

//            var listPostData = listPost.AsList();
//            var newestPost = new Post();

//            if (listPost != null && listPost.Count() > 0 && listPostData[0].PostID < newestPostID)
//            {
//                return listPost.AsList();
//            }

//            return null;

//        }

//    }
//}
