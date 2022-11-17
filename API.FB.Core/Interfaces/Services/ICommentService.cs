using API.FB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Interfaces.Services
{
    public interface ICommentService 
    {
        public int editComment(Comment comment);
    }
}
