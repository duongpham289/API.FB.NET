using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class Post 
    {
        public Guid PostID { get; set; }
        
        public Guid UserID { get; set; }

        public string? Content { get; set; }

        public string? Media { get; set; }
        
    }


    public class PostCustom : Post
    {
        /// <summary>
        /// Số like
        /// </summary>
        public int? like { get; set; }

        /// <summary>
        /// Số comment
        /// </summary>
        public int? comment { get; set; }

        /// <summary>
        /// đã like chưa
        /// </summary>
        public bool? is_liked { get; set; }

        /// <summary>
        /// Có bị block không
        /// </summary>
        public bool? is_blocked { get; set; }

        /// <summary>
        /// Có thể cmt
        /// </summary>
        public bool? can_cmt { get; set; }

        /// <summary>
        /// Có thể sửa k 
        /// </summary>
        public bool? can_edit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? banned { get; set; }

        /// <summary>
        /// ID tác giả
        /// </summary>
        public Guid? author_id { get; set; }

        /// <summary>
        /// Tên tác giả bài viết
        /// </summary>
        public string author_name { get; set; }

        /// <summary>
        /// Avatar tác giả
        /// </summary>
        public string author_avatar { get; set; }

        /// <summary>
        /// Trang thái online của tác giả
        /// </summary>
        public bool? author_onlike { get; set; }

        /// <summary>
        /// iđ bài post gần nhất
        /// </summary>
        public Guid? last_id { get; set; }


    }
}
