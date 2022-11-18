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
}
