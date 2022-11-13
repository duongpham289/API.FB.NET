using API.FB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Interfaces.Services
{
    public interface IBaseService<MISAEntity>
    {
        #region Methods

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="entity">Dữ liệu truyền vào</param>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        ServiceResult Add(MISAEntity entity);

        /// <summary>
        /// Sửa dữ liệu trong DataBase
        /// </summary>
        /// <param name="entity">Dữ liệu truyền vào</param>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        ServiceResult Update(MISAEntity entity, Guid entityId);
        #endregion
    }
}
