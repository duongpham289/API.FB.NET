using API.FB.Core.Entities;
using System;

namespace API.FB.Core.Interfaces.Services
{
    public interface IAuthService
    {

        /// <summary>
        /// Xử lí nghiệp vụ chung khi thêm mới đối tượng
        /// </summary>
        /// <param name="entity"> Đối tượng thêm mới </param>
        /// <returns> Số lượng bản ghi </returns>
        ServiceResult InsertService(User entity);
    }
}
