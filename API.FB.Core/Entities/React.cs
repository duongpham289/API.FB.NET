using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class React
    {
        public int ReactID { get; set; }
        public Guid UserID { get; set; }
        public int PostID { get; set; }
        public string? Token { get; set; }
        public int? Status { get; set; }
    }
}
