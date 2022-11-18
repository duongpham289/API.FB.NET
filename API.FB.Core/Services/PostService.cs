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
    public class PostService : IPostService
    {
        IPostRepo _postRepo;
        public PostService(IPostRepo postRepo)
        {
            _postRepo = postRepo;
        }
        public int InsertPost(Post post)
        {
            return _postRepo.InsertPost(post);
        }

        public int UpdatePost(Post post)
        {
            return _postRepo.UpdatePost(post);
        }


        public List<Post> GetPostList() { return new List<Post>(); }
        //public int Like(React react) { return _postRepo.ReactPost(react); }


        public int ReportPost(Report report)
        {
            return _postRepo.ReportPost(report);
        }
    }
}
