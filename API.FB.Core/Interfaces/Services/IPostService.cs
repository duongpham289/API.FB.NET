using API.FB.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Interfaces.Services
{
    public interface IPostService 
    {
        public Post GetPost(Post post);

        public int InsertPost(Post post);

        public int UpdatePost(Post post);


        public int ReportPost(Report report);

        public List<Post> GetPostList();
        public int React(React react);
        public ServiceResult ValidateBeforeRepo(ServiceResult result, string token, string described, List<IFormFile> imageList, IFormFile video);
    }
}
