using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNWTTBL.Entities
{
    public class Report
    {
        public Guid ReportID { get; set; }

        public Guid UserID { get; set; }

        public Guid ReportedUserID { get; set; }

    }
}
