using API.FB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Interfaces.Repository
{
    public interface IAuthRepo /*: IBaseRepo<User>*/
    {
        User getUserByPhoneNumber(Auth auth);

        /// <summary>
        /// Kiểm tra xem người dùng đã tồn tại hay chưa thông qua số điẹn thoại
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool CheckSignupLegal(User user);
    }
}
