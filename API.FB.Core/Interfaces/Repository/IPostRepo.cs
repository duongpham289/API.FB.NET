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
        object GetListPost(Post post, out List<Post> postResult, out List<MediaPost> postMedia);
        /// <summary>
        /// Lấy tất cả Post
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG
        object GetNewListPost(Post post, out List<Post> postResult, out List<MediaPost> postMedia);

        int DeletePost(Post post);

        int UpdatePost(Post post);
        int InsertPost(Post post);

        object GetPost(Post post, out List<Post> postResult, out List<MediaPost> postMedia);

        int ReportPost(Report report);

        int ReactPost(React react);

        bool GetPermissionPostAction(Post post);
    }
}
