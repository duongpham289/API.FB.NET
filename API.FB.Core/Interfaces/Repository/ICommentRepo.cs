using API.FB.Core.Entities;
using System;
using System.Collections.Generic;

namespace API.FB.Core.Interfaces.Repository
{
    public interface ICommentRepo : IBaseRepo<Comment>
    {
        /// <summary>
        /// Hàm gọi theo postID
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        List<Comment> GetByPostId(Comment comment);

        int InsertComment(Comment comment);
    }
}
