using CNWTTBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNWTTBL.Interfaces.Services
{
    public interface ICommentService 
    {
        public int editComment(Guid postId);
    }
}
