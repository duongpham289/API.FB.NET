using CNWTTBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNWTTBL.Interfaces.Repositories
{
    public interface ILikeRepo : IBaseRepo<Like>
    {

        //int LikeStatusChanged(Guid userID, Guid postID, int status);

        int DeleteCustom(Guid userID, Guid postID);

    }
}
