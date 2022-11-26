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
        List<Post> GetListPost(Post post);
        /// <summary>
        /// Lấy tất cả Post
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG
        List<Post> GetNewListPost(Post post);

        int DeletePost(Post post);

        int UpdatePost(Post post);
        int InsertPost(Post post);

        object GetPost(Post post, out List<Post> postResult, out List<MediaPost> postMedia);

        int ReportPost(Report report);

        int ReactPost(React react);
    }
}
