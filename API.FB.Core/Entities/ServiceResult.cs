using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class ServiceResult
    {
        #region ServiceResultProperty

        /// <summary>
        /// Hợp lệ 
        /// </summary>
        /// CreatedBY: PHDUONG
        public string code { get; set; }

        /// <summary>
        /// Thông tin đi kèm
        /// </summary>
        /// CreatedBY: PHDUONG
        public string message { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        /// CreatedBY: PHDUONG
        public object data { get; set; }



        public ServiceResult OnException(Exception ex)
        {
            if (ex != null)
            {
                this.code = "9999";
                this.message = "Lỗi Exception " + ex.Message;

            }

            return this;
        }
        #endregion
    }
}
