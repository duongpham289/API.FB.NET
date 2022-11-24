using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class Post : BaseEntity
    {
        public int PostID { get; set; }

        public Guid UserID { get; set; }

        public string? Token { get; set; }

        public string? Described { get; set; }

        public string? Media { get; set; }

        public int? Status { get; set; }

        /// <summary>
        /// Số like
        /// </summary>
        public int? ReactCount { get; set; }

        /// <summary>
        /// Số comment
        /// </summary>
        public int? CommentCount { get; set; }
        /// <summary>
        /// Số post mới
        /// </summary>
        public int? NewItems { get; set; }

        /// <summary>
        /// đã like chưa
        /// </summary>
        public bool? Is_liked { get; set; }

        /// <summary>
        /// Có bị block không
        /// </summary>
        public bool? Is_blocked { get; set; }

        /// <summary>
        /// Có thể cmt
        /// </summary>
        public bool? can_comment { get; set; }

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
        public Guid? Author_id { get; set; }

        /// <summary>
        /// Tên tác giả bài viết
        /// </summary>
        public string Author_name { get; set; }

        /// <summary>
        /// Avatar tác giả
        /// </summary>
        public string Author_avatar { get; set; }

        /// <summary>
        /// Trang thái online của tác giả
        /// </summary>
        public bool? author_onlike { get; set; }

        /// <summary>
        /// iđ bài post gần nhất
        /// </summary>
        public int? LatestPostID { get; set; }
        /// <summary>
        /// iđ bài post gần nhất
        /// </summary>
        public int? PageIndex { get; set; }
        /// <summary>
        /// iđ bài post gần nhất
        /// </summary>
        public int? PageCount{ get; set; }

    }
}
