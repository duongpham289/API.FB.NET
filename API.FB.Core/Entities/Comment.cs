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

        public string? content { get; set; }

        public int? index { get; set; }
        public int? count { get; set; }
    }
}
