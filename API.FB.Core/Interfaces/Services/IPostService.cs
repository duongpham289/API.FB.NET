using API.FB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Interfaces.Services
{
    public interface IPostService 
    {
        public int InsertPost(Post post);

        public int UpdatePost(Guid postId ,Post post);

        public int DeletePost(Guid postId);

        public bool ReportPost(Guid postId);

        public List<Post> GetPostList();
        public int Like(Guid postId);
    }
}
