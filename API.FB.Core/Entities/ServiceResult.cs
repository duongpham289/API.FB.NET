﻿using System;
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
        public int ResponseCode { get; set; }

        /// <summary>
        /// Thông tin đi kèm
        /// </summary>
        /// CreatedBY: PHDUONG
        public string Message { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        /// CreatedBY: PHDUONG
        public object Data { get; set; }



        public ServiceResult OnException(Exception ex)
        {
            if (ex != null)
            {
                this.ResponseCode = 9999;
                this.Message = "Lỗi Exception " + ex.Message;

            }

            return this;
        }
        #endregion
    }
}
