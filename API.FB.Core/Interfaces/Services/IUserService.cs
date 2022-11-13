using API.FB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Interfaces.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Lấy danh sách phân trang
        /// </summary>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="employeeFilter">Dữ liệu lọc phân trang</param>
        /// <param name="check">Check export hay paging</param>
        /// <returns></returns>
        /// CreatedBy: PHDUONG
        dynamic ExportUser(int pageIndex, int pageSize, string userFilter, bool check);
    }
}
