using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class React
    {
        public Guid ReactID { get; set; }
        public Guid UserID { get; set; }
        public Guid PostId { get; set; }
        public int? Status { get; set; }
    }
}
