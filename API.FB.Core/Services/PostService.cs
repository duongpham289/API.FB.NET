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

                result.ResponseCode = 9998;
                result.Message = "Sai token";
                return result;
            }

            User user = _userRepository.GetUserByToken(token);
            if (user == null)
            {
                result.ResponseCode = 1009;
                result.Message = "Không có quyền truy cập tài nguyên";
                return result;
            }

            if (String.IsNullOrWhiteSpace(described))
            {
                result.ResponseCode = 1002;
                result.Message = "Số lượng Parameter không đầy đủ";
                return result;
            }

            if (described?.Length > 65.535)
            {
                result.ResponseCode = 1006;
                result.Message = "Độ dài đầu vào quá mức cho phép";
                return result;
            }

            if (imageList != null && imageList.Count > 0)
            {
                if (video != null)
                {
                    result.ResponseCode = 1007;
                    result.Message = "Upload thất bại, chỉ được upload ảnh hoặc video";
                    return result;
                }
                else if (imageList.Count > 5)
                {
                    result.ResponseCode = 1008;
                    result.Message = "Số lượng ảnh vượt quá quy định";
                    return result;
                }
            }
            else if (video != null)
            {
                if (video.Count > 1)
                {
                    result.ResponseCode = 1008;
                    result.Message = "Số lượng video vượt quá quy định";
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
                                    result.ResponseCode = 1007;
                                    result.Message = "Upload thất bại, định dạng file không hợp lệ";
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
                                    result.ResponseCode = 1007;
                                    result.Message = "Upload thất bại, định dạng file không hợp lệ";
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
                                    result.ResponseCode = 1006;
                                    result.Message = "Cỡ file vượt mức cho phép";
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
                                    result.ResponseCode = 1007;
                                    result.Message = "Cỡ file vượt mức cho phép";
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
