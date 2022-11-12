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
    public class CommentService : BaseService<Comment>, ICommentService
    {
        ICommentRepo _commentRepo;
        public CommentService(ICommentRepo commentRepo) : base(commentRepo)
        {
            _commentRepo = commentRepo;
        }

        public int editComment(Guid postId)
        {
            return 1;
        }
    }
}
