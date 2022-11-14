using API.FB.Core.FBAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class User : BaseEntity
    {
        #region UserProperty

        /// <summary>
        /// Khóa chính
        /// </summary>
        /// CreatedBy:PHDUONG
        public Guid UserID { get; set; }

        //[MISAPropExport(("Mã nhân viên"))]
        //[MISARequired("Mã nhân viên")]
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        /// CreatedBy:PHDUONG
        public string Password { get; set; }

        //[MISAPropExport(("Tên nhân viên"))]
        //[MISARequired("Họ và tên")]
        /// <summary>
        /// Họ và tên
        /// </summary>
        /// CreatedBy:PHDUONG
        public string FullName { get; set; }


        /// <summary>
        /// Số điện thoại Di động
        /// </summary>
        /// CreatedBy:PHDUONG
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Avatar
        /// </summary>
        /// CreatedBy:PHDUONG
        public string Avatar { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        /// CreatedBy:PHDUONG
        public string Token { get; set; }

        #endregion
    }
}
