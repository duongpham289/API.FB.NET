using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNWTTBL.Interfaces.Services
{
    public interface IBaseService<T>
    {

        /// <summary>
        /// Xử lí nghiệp vụ chung khi thêm mới đối tượng
        /// </summary>
        /// <param name="entity"> Đối tượng thêm mới </param>
        /// <returns> Số lượng bản ghi </returns>
        int InsertService(T entity);

        /// <summary>
        /// Xử lí nghiệp vụ khi sửa dối tượng
        /// </summary>
        /// <param name="entityId"> Id của đối tượng </param>
        /// <param name="entity"> đối tượng đã sửa </param>
        /// <returns></returns>
        int UpdateService(Guid entityId, T entity);
    }
}
