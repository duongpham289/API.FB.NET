using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Entities;
using API.FB.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using System.IO;

namespace CNWTT.Controllers
{
    [Route("fb")]
    [ApiController]
    public class PostController : ControllerBase
    {

        IPostService _postService;
        IPostRepo _postRepo;

        IUserRepository _userRepository;

        public PostController(IPostService postService, IPostRepo postRepo, IUserRepository userRepository)
        {
            _postService = postService;
            _postRepo = postRepo;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Lấy tất cả Code
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_list_post")]
        public virtual ServiceResult GetListPost([FromQuery] Post post)
        {
            ServiceResult result = new ServiceResult();
            try
            {

                var token = post.Token;
                var latestPostID = post.LatestPostID;
                var pageCount = post.PageCount;
                var pageIndex = post.PageIndex;

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }

                if (latestPostID == null || pageCount == null || pageIndex == null)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                var listPost = _postRepo.GetListPost(post);


                var listDisplayPost = new List<object>();

                foreach (var item in listPost)
                {
                    byte[] bytes = Convert.FromBase64String(item.Image);

                    Image image;
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = Image.FromStream(ms);
                    }

                    var temp = new
                    {
                        PostID = item.PostID,
                        Described = item.Described,
                        Created = item.CreatedDate,
                        Modified = item.ModifiedDate,
                        Like = item.ReactCount,
                        Comment = item.CommentCount,
                        Is_liked = item.Is_liked,
                        Image = image,
                        Author = new
                        {
                            AuthorID = item.Author_id,
                            AuthorName = item.Author_name,
                            AuthorAvatar = item.Author_avatar,
                        },
                        Is_blocked = item.Is_blocked

                    };

                    listDisplayPost.Add(temp);
                }

                result.ResponseCode = 1000;
                result.Data = new
                {
                    posts = listDisplayPost,
                    NewItems = listPost[0].NewItems,
                    LastID = listPost[0].PostID,

                };
                result.Message = "OK";
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }

        /// <summary>
        /// Lấy tất cả Code
        /// </summary>
        /// <returns></returns>
        [HttpGet("check_new_item")]
        public ServiceResult GetNewListPost([FromQuery] Post post)
        {

            ServiceResult result = new ServiceResult();
            try
            {
                var token = post.Token;
                var latestPostID = post.LatestPostID;

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }

                if (latestPostID == null)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }
                var listPost = _postRepo.GetNewListPost(post);

                var listDisplayPost = new List<object>();

                foreach (var item in listPost)
                {
                    byte[] bytes = Convert.FromBase64String(item.Image);

                    Image image;
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = Image.FromStream(ms);
                    }

                    var temp = new
                    {
                        PostID = item.PostID,
                        Described = item.Described,
                        Created = item.CreatedDate,
                        Modified = item.ModifiedDate,
                        Like = item.ReactCount,
                        Comment = item.CommentCount,
                        Is_liked = item.Is_liked,
                        image,
                        Author = new
                        {
                            AuthorID = item.Author_id,
                            AuthorName = item.Author_name,
                            AuthorAvatar = item.Author_avatar,
                        },
                        Is_blocked = item.Is_blocked

                    };

                    listDisplayPost.Add(temp);
                }

                result.ResponseCode = 1000;
                result.Data = new
                {
                    //posts = listDisplayPost,
                    NewItems = listPost[0].NewItems,
                    //LastID = listPost[0].PostID,

                };
                result.Message = "OK";
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }

        /// <summary>
        /// Lấy về bài viết cụ thể
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        [HttpGet("get_post")]
        public ServiceResult GetPost([FromQuery] Post post)
        {

            ServiceResult result = new ServiceResult();
            try
            {
                var token = post.Token;
                var postID = post.PostID;

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }

                if (postID == null)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }


                //dic.PostID = Guid.NewGuid();
                var postResult = _postService.GetPost(post);

                byte[] bytes = Convert.FromBase64String(postResult.Image);

                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);

                    using (Bitmap bm2 = new Bitmap(ms))
                    {
                        bm2.Save("C:\\Users\\DUONG.PH187315\\Desktop\\Pic\\" + "API_FB.jpg");
                    }
                }


                result.ResponseCode = 1000;
                result.Message = "OK";
                result.Data = new
                {
                    PostID = postResult.PostID,
                    Described = postResult.Described,
                    Created = postResult.CreatedDate,
                    Modified = postResult.ModifiedDate,
                    Like = postResult.ReactCount,
                    Comment = postResult.CommentCount,
                    Is_liked = postResult.Is_liked,
                    image,
                    Author = new
                    {
                        AuthorID = postResult.Author_id,
                        AuthorName = postResult.Author_name,
                        AuthorAvatar = postResult.Author_avatar,
                    },
                    Is_blocked = postResult.Is_blocked,

                };

                return result;
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }

        /// <summary>
        /// Tạo bài viết mới
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        [HttpPost("add_post")]
        public ServiceResult Post([FromForm] Post post)
        {

            ServiceResult result = new ServiceResult();
            try
            {
                var token = post.Token;
                var described = post.Described;
                var media = post.Media;
                var status = post.Status;

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }

                if (String.IsNullOrWhiteSpace(described) || String.IsNullOrWhiteSpace(token))
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


                //dic.PostID = Guid.NewGuid();
                var postID = _postService.InsertPost(post);

                result.ResponseCode = 1000;
                result.Message = "OK";
                result.Data = new { PostID = postID };

                return result;
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }

        /// <summary>
        /// Xử lí sửa đối tượng 
        /// </summary>
        /// <param name="entityId"> Id của đôi tượng </param>
        /// <param name="entity"> Dữ liệu mới </param>
        /// <returns></returns>
        // PUT api/<MISABaseController>/5
        [HttpPut("edit_post")]
        public ServiceResult Put([FromQuery] Post post)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var token = post.Token;
                var postID = post.PostID;
                var described = post.Described;
                var media = post.Media;
                var status = post.Status;

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }

                if (String.IsNullOrWhiteSpace(described) || postID == null)
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

                _postService.UpdatePost(post);

                result.ResponseCode = 1000;
                result.Message = "OK";
                return result;
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }

        /// <summary>
        /// Xử lí xóa đối tượng theo Id
        /// </summary>
        /// <param name="entityId"> Id của đối tượng </param>
        /// <returns></returns>
        [HttpDelete("delete_post")]
        public ServiceResult Delete([FromQuery] Post post)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var token = post.Token;
                var postID = post.PostID.ToString();

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }

                if (String.IsNullOrWhiteSpace(postID))
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }


                var serviceResult = _postRepo.DeletePost(post);

                result.ResponseCode = 1000;
                result.Message = "OK";
                return result;
            }
            catch (Exception ex)
            {
                result.OnException(ex);

            }
            return result;

        }

        /// <summary>
        /// Controller like
        /// </summary>
        /// <param name="react"></param>
        /// <returns></returns>
        [HttpPost("react_post")]
        public ServiceResult LikeStatusChanged([FromQuery] React react)
        {
            ServiceResult result = new ServiceResult();
            try
            {

                var token = react.Token;
                var postID = react.PostID.ToString();

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }

                if (String.IsNullOrWhiteSpace(postID))
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }
                //react.ReactID = Guid.NewGuid();
                var likeCount = _postService.React(react);
                result.ResponseCode = 1000;
                result.Message = "OK";
                result.Data = new
                {
                    LikeCount = likeCount
                };
                return result;
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;
        }


        [HttpPost("report_post")]
        public ServiceResult ReportPost([FromQuery] Report report)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var token = report.Token;
                var postID = report.PostID;
                var details = report.Details;
                var subject = report.Subject;

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }

                if (String.IsNullOrWhiteSpace(details) || String.IsNullOrWhiteSpace(subject) || postID == null)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                if (subject.Length > 255 || details.Length > 65.535)
                {
                    result.ResponseCode = 1006;
                    result.Message = "Độ dài đầu vào quá mức cho phép";
                    return result;
                }

                _postService.ReportPost(report);

                result.ResponseCode = 1000;
                result.Message = "OK";
                return result;
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }
    }
}
