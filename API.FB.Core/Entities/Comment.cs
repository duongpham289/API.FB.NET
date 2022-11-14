using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNWTTBL.Entities
{
    public class Comment 
    {
        public Guid CommentID { get; set; }

        public Guid PostID { get; set; }

        public string? CommentContent { get; set; }
    }
}
