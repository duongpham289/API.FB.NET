using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class Report
    {
        public Guid ReportID { get; set; }
        public Guid UserID { get; set; }
        public Guid PostID { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }


    }
}
