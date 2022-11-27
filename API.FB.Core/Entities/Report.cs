using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class Report
    {
        public int ReportID { get; set; }
        public Guid UserID { get; set; }
        public int? PostID { get; set; }
        public string? Token { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }


    }
}
