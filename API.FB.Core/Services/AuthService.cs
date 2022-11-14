using API.FB.Core.Interfaces.Repository;
using CNWTTBL.Entities;
using CNWTTBL.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNWTTBL.Services
{
    public class AuthService : IAuthService
    {
        IAuthRepo _authRepo;
        public AuthService(IAuthRepo authRepo) 
        {
            _authRepo = authRepo;
        }
    }
}
