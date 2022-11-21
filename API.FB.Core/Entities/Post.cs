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
        public int PostID { get; set; }
        
        public Guid UserID { get; set; }

        public string? Token { get; set; }

        public string? Described { get; set; }

        public string? Media { get; set; }

        public int? Status { get; set; }
        
    }
}
