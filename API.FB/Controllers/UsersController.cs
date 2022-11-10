using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.FB.Core.Entities;
using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Interfaces.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.FB.Controllers
{
    [Route("fb")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region DECLARE

        IUserService _employeeService;
        IUserRepository _employeeRepository;
        #endregion

        #region Constructor
        public UsersController(IUserService employeeService, IUserRepository employeeRepository)
        {
            _employeeService = employeeService;
            _employeeRepository = employeeRepository;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Lấy thông tin phân trang nhân viên
        /// </summary>
        /// <returns>Dữ liệu phân trang</returns>
        /// CreatedBy:PHDUONG(27/08/2021)
        [HttpGet("paging")]
        public IActionResult GetUsersPaging([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string employeeFilter)
        {
            try
            {
                var serviceResult = _employeeRepository.GetPaging(pageIndex, pageSize, employeeFilter, true);

                if (serviceResult.GetType().GetProperty("totalRecord").GetValue(serviceResult, null) > 0)
                    return StatusCode(200, serviceResult);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = API.FB.Core.Resources.ResourceVN.ExceptionError_Msg,
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }

        }

        /// <summary>
        /// Xuất Dữ liệu dạng Excel
        /// </summary>
        /// <returns></returns>
        /// CreatedBy:PHDUONG(03/09/2021)
        [HttpGet("export")]
        public IActionResult Export([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string employeeFilter)
        {

            var stream = _employeeService.ExportUser(pageIndex, pageSize, employeeFilter, false);

            string excelName = $"DanhSachNhanVien.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        /// <summary>
        /// Lấy tất cả Code
        /// </summary>
        /// <returns></returns>
        ///  CreatedBy: PHDUONG(27/08/2021)
        [HttpGet("getAllCode")]
        public virtual IActionResult GetAllCode()
        {
            try
            {
                var entityCode = _employeeRepository.GetAllUserCode();

                if (entityCode != null)
                {
                    return StatusCode(200, entityCode);

                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = API.FB.Core.Resources.ResourceVN.ExceptionError_Msg,
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
        }

        /// <summary>
        /// Lấy Code cho nhân viên mới
        /// </summary>
        /// <returns></returns>
        ///  CreatedBy: PHDUONG(27/08/2021)
        [HttpGet("getNewCode")]
        public virtual IActionResult GetNewCode()
        {
            try
            {
                var entityCode = _employeeRepository.GetNewUserCode();

                if (entityCode != null)
                {
                    return StatusCode(200, entityCode);

                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = API.FB.Core.Resources.ResourceVN.ExceptionError_Msg,
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
        }
        #endregion
    }
}
