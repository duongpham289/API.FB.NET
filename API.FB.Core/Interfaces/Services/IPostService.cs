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
        public ServiceResult ValidateBeforeRepo(ServiceResult result, string token, string described, List<IFormFile> imageList, List<IFormFile> video);

    }
}
