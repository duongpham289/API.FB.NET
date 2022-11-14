//using Dapper;
//using Microsoft.Extensions.Configuration;
//using System;

//namespace CNWTTDL.Repository
//{
//    public class LikeRepo 
//    {


//        #region Constructor
//        public LikeRepo(IConfiguration configuration) : base(configuration)
//        {

//        }

//        public int DeleteCustom(Guid userID, Guid postID)
//        {  // Khỏi tạo câu truy vấn:
//            var sqlQuery = $"DELETE FROM like_post WHERE UserID = @userID and PostID = @postID";
//            var parameters = new DynamicParameters();
//            parameters.Add("@userID", userID);
//            parameters.Add("@postID", postID);
//            // Thực hiện câu truy vấn: 
//            var res = _sqlConnection.Execute(sqlQuery, param: parameters);
//            return res;
//        }
//        #endregion




//    }
//}
