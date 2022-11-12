using CNWTTBL.Core.BaseAttribute;
using CNWTTBL.Entities;
using CNWTTBL.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Web06.NSDH.Core.Enum;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNWTTDL.Repository
{
    public class LikeRepo : BaseRepo<Like>, ILikeRepo
    {


        #region Constructor
        public LikeRepo(IConfiguration configuration) : base(configuration)
        {

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
        #endregion




    }
}
