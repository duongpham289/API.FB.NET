using API.FB.Core.Entities;
using System;
using System.Collections.Generic;

namespace API.FB.Core.Interfaces.Repository
{
    public interface IPostRepo 
    {

        /// <summary>
        /// Lấy tất cả Post
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG
        List<Post> GetListPost(string token, Guid userID, Guid lastedPostID, int skip, int take);
        /// <summary>
        /// Lấy tất cả Post
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG
        List<Post> GetNewListPost(string token, Guid lastedPostID);

        int DeletePost(Post post);

        int UpdatePost(Post post);

        int InsertPost(Post post);

        int ReportPost(Report report);

        int LikePost(string token, Guid postID);

    }
}
