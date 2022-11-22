using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class Comment 
    {
        public int CommentID { get; set; }

        public int PostID { get; set; }

        public Guid UserID { get; set; }

        public string? Token { get; set; }

        public string? CommentContent { get; set; }

        public int? Index { get; set; }
        public int? Count { get; set; }
    }
}
