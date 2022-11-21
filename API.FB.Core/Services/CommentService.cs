using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Entities;
using API.FB.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Services
{
    public class CommentService : ICommentService
    {
        ICommentRepo _commentRepo;
        public CommentService(ICommentRepo commentRepo) 
        {
            _commentRepo = commentRepo;
        }

        public int InsertComment(Comment comment)
        {
            return _commentRepo.InsertComment(comment);
        }
    }
}
