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
    public class PostService : IPostService
    {
        IPostRepo _postRepo;
        public PostService(IPostRepo postRepo) 
        {
            _postRepo = postRepo;
        }
        public int InsertPost(Post post)
        {
            return 1;
        }

        public int UpdatePost(Guid postId, Post post)
        {
               return -1;
        }

        public int DeletePost(Guid postId) { return 0; }

        public List<Post> GetPostList() { return new List<Post>(); }
        public int Like(Guid postId) { return 0;}
    }
}
