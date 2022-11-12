using CNWTTBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNWTTBL.Interfaces.Repositories
{
    public interface IAuthRepo : IBaseRepo<User>
    {
        User getUserByEmail(Auth auth);
    }
}
