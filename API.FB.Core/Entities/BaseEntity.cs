using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class BaseEntity
    {
        #region BaseProperty
        /// <summary>
        /// Ngày Tạo
        /// </summary>
        /// CreatedBy:PHDUONG
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        /// CreatedBy:PHDUONG
        public string CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        /// CreatedBy:PHDUONG
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        /// CreatedBy:PHDUONG
        public string ModifiedBy { get; set; }

        #endregion
    }
}
