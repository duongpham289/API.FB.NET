using API.FB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Interfaces.Repository
{
    public interface IUserRepository
    {
        #region Methods

        /// <summary>
        /// Lấy danh sách người dùng phân trang
        /// </summary>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="userFilter">Dữ liệu lọc phân trang</param>
        /// <returns></returns>
        /// CreatedBy: PHDUONG
        dynamic GetPaging(int pageIndex, int pageSize, string employeeFilter, bool check);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG
        List<string> GetAllUserCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG
        string GetNewUserCode();

        /// <summary>
        /// Check trùng 
        /// </summary>
        /// <param name="entityProperty">Thuộc tính thực thể</param>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        bool IsDuplicated(string entityProperty);

        /// <summary>
        /// Thêm mới user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Guid Insert(User user);

        /// <summary>
        /// Sửa user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int Update(User user);

        /// <summary>
        /// Sửa user token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int UpdateTokenForUser(User user);

        /// <summary>
        /// Gọi user theo token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        User GetUserByToken(string token);

        #endregion
    }
}
