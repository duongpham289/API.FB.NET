using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class Comment : BaseEntity
    {
        public int CommentID { get; set; }

        public int? PostID { get; set; }

        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }

        public string? Token { get; set; }

        public string? CommentContent { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
