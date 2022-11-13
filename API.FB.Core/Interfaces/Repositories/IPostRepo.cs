﻿using CNWTTBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNWTTBL.Interfaces.Repositories
{
    public interface IPostRepo : IBaseRepo<Post>
    {

        /// <summary>
        /// Lấy tất cả Post
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG
        List<Post> GetListPost(Guid userID);
        /// <summary>
        /// Lấy tất cả Post
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG
        List<Post> GetNewListPost(Guid userID, int newestPostID);
    }
}