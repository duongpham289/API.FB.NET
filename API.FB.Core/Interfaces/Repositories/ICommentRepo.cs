using CNWTTBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNWTTBL.Interfaces.Repositories
{
    public interface ICommentRepo : IBaseRepo<Comment>
    {
        /// <summary>
        /// Hàm gọi theo postID
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        List<Comment> GetByPostId(Guid postId);
    }
}
