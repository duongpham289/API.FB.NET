using CNWTTBL.Entities;
using CNWTTBL.Interfaces.Repositories;
using CNWTTBL.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNWTTBL.Services
{
    public class AuthService : BaseService<User>, IAuthService
    {
        IAuthRepo _authRepo;
        public AuthService(IAuthRepo authRepo) : base(authRepo)
        {
            _authRepo = authRepo;
        }
    }
}
