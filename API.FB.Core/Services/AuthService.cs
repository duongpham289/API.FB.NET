using API.FB.Core.Entities;
using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Services
{
    public class AuthService : IAuthService
    {
        IAuthRepo _authRepo;
        public AuthService(IAuthRepo authRepo) 
        {
            _authRepo = authRepo;
        }


        /// <summary>
        /// Kiểm tra dữ liệu khi insert
        /// </summary>
        /// <param name="entity">entity nhân viên để kiểm tra</param>
        /// <returns></returns>
        /// <exception cref="exception"></exception>
        /// CreatedBy: BDAnh(08/11/2022)
        public ServiceResult InsertService(User entity)
        {

            ServiceResult result = new ServiceResult();
            try
            {
                //Thực hiện validate dữ liệu
                var isValid = Validate(entity);
                if (isValid.Count() == 0)
                {
                    //result.Data = _baseRepo.Insert(entity);
                    result.ResponseCode = 1000;
                    return result;
                }
                else
                {
                    result.ResponseCode = 9996;
                    result.Message = "Người dùng đã tồn tại";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }

        /// <summary>
        /// Validate chung
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy: BDAnh(08/11/2022)
        protected List<string> Validate(User user)
        {
            List<string> errors = new List<string>();
            //ValidationContext context = new ValidationContext(entity);
            //var validateEntity = new List<ValidationResult>();
            //bool result = Validator.TryValidateObject(entity, context, validateEntity, true);
            //if (!result)
            //{
            //    validateEntity.ToList().ForEach((error) =>
            //    {
            //        errors.Add(error.ErrorMessage);
            //    });
            //};

            return errors;
        }
    }


}
