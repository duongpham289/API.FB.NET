﻿using CNWTTBL.Entities;
using System;
using System.Collections.Generic;

namespace API.FB.Core.Interfaces.Repository
{
    public interface ICommentRepo : IBaseRepository<Comment>
    {
        /// <summary>
        /// Hàm gọi theo postID
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        List<Comment> GetByPostId(Guid postId);
    }
}
