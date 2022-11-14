using API.FB.Core.Entities;
using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Interfaces.Services;
using API.FB.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Services
{
    public class BaseService<MISAEntity> : IBaseService<MISAEntity>
    {
        #region DECLARE

        IBaseRepository<MISAEntity> _baseRepository;
        protected ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<MISAEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="entity">Dữ liệu truyền vào</param>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(16/08/2021)
        public ServiceResult Add(MISAEntity entity)
        {

            if (!ValidateData(entity))
            {
                _serviceResult.IsValid = ValidateData(entity);
                return _serviceResult;
            }
            else if (!ValidateCustom(entity))
            {
                return _serviceResult;
            }

            _serviceResult.Data = _baseRepository.Add(entity);

            return _serviceResult;

        }

        /// <summary>
        /// Sửa dữ liệu trong DataBase
        /// </summary>
        /// <param name="entity">Dữ liệu truyền vào</param>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(16/08/2021)
        public ServiceResult Update(MISAEntity entity, Guid entityId)
        {
            if (!ValidateData(entity))
            {
                _serviceResult.IsValid = ValidateData(entity);
                return _serviceResult;
            }
            else if (!ValidateCustom(entity))
            {
                return _serviceResult;
            }

            _serviceResult.Data = _baseRepository.Update(entity, entityId);

            return _serviceResult;
        }

        #endregion

        #region ValidateMethods
        /// <summary>
        /// Xử lí validate dữ liêu chung
        /// </summary>
        /// <param name="entity">đối tượng muốn thưc hiện validate</param>
        /// <returns>true - dữ liệu hơp lệ, false - dữ liệu không hợp lệ</returns>
        /// CreatedBy: PHDUONG(18/08/2021)
        private bool ValidateData(MISAEntity entity)
        {
            var isValid = true;

            // 1. Lay thong tin cac properties:
            var properties = typeof(MISAEntity).GetProperties();

            //2. Xac dinh  viec validate dựa trên attribute: (MISARequired - bat buoc check thong tin, khong duoc phep null  hoac de trong)
            foreach (var prop in properties)
            {
                var propValue = prop.GetValue(entity);

                var propName = prop.Name;

                //Kiem tra xem property hien tai co bat buoc nhap hay khong
                var propMISARequireds = prop.GetCustomAttributes(typeof(MISARequired), true);
                if (propMISARequireds.Length > 0)
                {
                    var message = (propMISARequireds[0] as MISARequired)._message;
                    if (prop.PropertyType == typeof(string) && (propValue == null || propValue.ToString() == string.Empty))
                    {
                        isValid = false;
                        _serviceResult.Message = message;
                        return isValid;
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// Check dữ liêu trước khi thêm mới
        /// </summary>
        /// <param name="entity">Thực thể cần check</param>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(17/08/2021)
        protected virtual bool ValidateCustom(MISAEntity entity)
        {
            var isValid = true;

            return isValid;
        }
        #endregion
    }
}
