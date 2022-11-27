using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Entities;
using API.FB.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using API.FB.Core.FBAttribute;

namespace API.FB.Core.Services
{
    public class PostService : IPostService
    {
        IUserRepository _userRepository;
        IPostRepo _postRepo;
        public PostService(IPostRepo postRepo, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _postRepo = postRepo;
        }

        public ServiceResult ValidateBeforeRepo(ServiceResult result, string token, string described, List<IFormFile> imageList, List<IFormFile> video)
        {
            if (token == null)
            {

                result.code = "9998";
                result.message = "Token is invalid";
                return result;
            }

            User user = _userRepository.GetUserByToken(token);
            if (user == null)
            {
                result.code = "1009";
                result.message = "Not access";
                return result;
            }

            if (String.IsNullOrWhiteSpace(described))
            {
                result.code = "1002";
                result.message = "Parameter is not enough";
                return result;
            }

            if (described?.Length > 65.535)
            {
                result.code = "1006";
                result.message = "Parameter value is invalid";
                return result;
            }

            if (imageList != null && imageList.Count > 0)
            {
                if (video != null)
                {
                    result.code = "1007";
                    result.message = "Upload File Failed!";
                    return result;
                }
                else if (imageList.Count > 4)
                {
                    result.code = "1008";
                    result.message = "Maximum number of images";
                    return result;
                }
            }
            else if (video != null)
            {
                if (video.Count > 1)
                {
                    result.code = "1008";
                    result.message = "Maximum number of images";
                    return result;
                }
            }

            return result;
        }

        public ServiceResult ValidateFile(ServiceResult result, Post post)
        {

            var properties = typeof(Post).GetProperties();

            foreach (var prop in properties)
            {
                var propValue = prop.GetValue(post);

                var propName = prop.Name;

                var propExtension = prop.GetCustomAttributes(typeof(AllowedExtensionsAttribute), true);
                if (propExtension.Length > 0)
                {
                    if (propName == "Image" && propValue != null)
                    {
                        try
                        {
                            foreach (var image in (List<IFormFile>)propValue)
                            {
                                var message = (propExtension[0] as AllowedExtensionsAttribute).IsValid(image);
                                if (!message)
                                {
                                    result.code = "1007";
                                    result.message = "Upload File Failed!";
                                    return result;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            result.OnException(ex);
                        }
                    }
                    else if(propName == "Video" && propValue != null)
                    {
                        try
                        {

                            foreach (var propVid in (List<IFormFile>)propValue)
                            {
                                var message = (propExtension[0] as AllowedExtensionsAttribute).IsValid(propVid);
                                if (!message)
                                {
                                    result.code = "1007";
                                    result.message = "Upload File Failed!";
                                    return result;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result.OnException(ex);
                        }

                    }
                }

                var propMaxSize = prop.GetCustomAttributes(typeof(MaxFileSizeAttribute), true);
                if (propMaxSize.Length > 0)
                {
                    if (propName == "Image" && propValue != null)
                    {
                        try
                        {
                            foreach (var image in (List<IFormFile>)propValue)
                            {
                                var message = (propMaxSize[0] as MaxFileSizeAttribute).IsValid(image);
                                if (!message)
                                {
                                    result.code = "1006";
                                    result.message = "File size is too big";
                                    return result;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            result.OnException(ex);
                        }
                    }
                    else if(propName == "Video" && propValue != null)
                    {
                        try
                        {
                            foreach (var propVid in (List<IFormFile>)propValue)
                            {
                                var message = (propMaxSize[0] as MaxFileSizeAttribute).IsValid(propVid);
                                if (!message)
                                {
                                    result.code = "1007";
                                    result.message = "File size is too big";
                                    return result;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            result.OnException(ex);
                        }

                    }
                }
            }

            return result;

        }



    }
}
