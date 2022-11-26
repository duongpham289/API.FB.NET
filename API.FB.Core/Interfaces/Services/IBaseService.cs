using API.FB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Interfaces.Services
{
    public interface IBaseService<FBEntity>
    {
        #region Methods

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="entity">Dữ liệu truyền vào</param>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        int InsertService(FBEntity entity);

        /// <summary>
        /// Sửa dữ liệu trong DataBase
        /// </summary>
        /// <param name="entity">Dữ liệu truyền vào</param>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        int UpdateService(Guid entityId, FBEntity entity);
        #endregion
    }
}
