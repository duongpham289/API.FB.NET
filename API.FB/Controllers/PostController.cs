using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Entities;
using API.FB.Core.Interfaces.Services;
using API.FB.Core.FBAttribute;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Http;

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
                var imageList = post.Image;
                var video = post.Video;
                var status = post.Status;

                var properties = typeof(Post).GetProperties();

                result = _postService.ValidateFile(result: result, post: post);

                result = _postService.ValidateBeforeRepo(result: result, token: token, described: described, imageList: imageList, video: video);

                if (result.ResponseCode != 0)
                {
                    return result;
                }

                var postID = _postRepo.InsertPost(post);

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
        /// Lấy về bài viết cụ thể
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        [HttpGet("get_post")]
        public ServiceResult GetPost([FromForm] Post post)
        {

            ServiceResult result = new ServiceResult();
            try
            {
                var token = post.Token;
                var postID = post.PostID;

                _postService.ValidateBeforeRepo(result: result, token: token, described: "hello", imageList: null, video: null);

                if (result.ResponseCode != 0)
                {
                    return result;
                }

                var postResult = new List<Post>();
                var postMedia = new List<MediaPost>();

                var res = _postRepo.GetPost(post, out postResult, out postMedia);

                if (postResult.Count <= 0)
                {
                    result.ResponseCode = 9992;
                    result.Message = "Bài viết khồng tồn tại";
                    return result;
                }

                string path = "C:\\Users\\DUONG.PH187315\\Desktop\\Pic\\";

                List<Image> imageList = new List<Image>();
                List<string> videoList = new List<string>();

                if (postMedia.Count > 0)
                {
                    if ((bool)postMedia[0].IsImage)
                    {
                        int index = 0;
                        foreach (var media in postMedia)
                        {
                            byte[] bytes = Convert.FromBase64String(media.Image);

                            try
                            {
                                using (MemoryStream ms = new MemoryStream(bytes))
                                {
                                    var image = Image.FromStream(ms);


                                    using (Bitmap bm2 = new Bitmap(ms))
                                    {
                                        bm2.Save(path + "PostID_" + postResult[0].PostID + "_Image_" + index + ".jpg");
                                    }

                                    imageList.Add(image);

                                }
                            }
                            catch (Exception ex)
                            {
                                result.OnException(ex);
                            }
                            index++;
                        }


                    }
                    else
                    {
                        try
                        {
                            FileInfo video = new FileInfo(path + "PostID_" + postResult[0].PostID + "_Video.mp4");
                            byte[] bytes = Convert.FromBase64String(postMedia[0].Video);

                            using (Stream sw = video.OpenWrite())
                            {
                                sw.Write(bytes, 0, bytes.Length);
                                sw.Close();
                            }

                            videoList.Add(video.FullName);
                        }
                        catch (Exception ex)
                        {
                            result.OnException(ex);
                        }
                    }

                }


                result.ResponseCode = 1000;
                result.Message = "OK";
                result.Data = new
                {
                    PostID = postResult[0].PostID,
                    Described = postResult[0].Described,
                    Created = postResult[0].CreatedDate,
                    Modified = postResult[0].ModifiedDate,
                    Like = postResult[0].ReactCount,
                    Comment = postResult[0].CommentCount,
                    Is_liked = postResult[0].Is_liked,
                    Image = imageList,
                    Video = videoList,
                    Author = new
                    {
                        AuthorID = postResult[0].Author_id,
                        AuthorName = postResult[0].Author_name,
                        AuthorAvatar = postResult[0].Author_avatar,
                    },
                    Is_blocked = postResult[0].Is_blocked,

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
        /// Xử lí sửa đối tượng 
        /// </summary>
        /// <param name="entityId"> Id của đôi tượng </param>
        /// <param name="entity"> Dữ liệu mới </param>
        /// <returns></returns>
        // PUT api/<MISABaseController>/5
        [HttpPut("edit_post")]
        public ServiceResult Put([FromForm] Post post)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var token = post.Token;
                var postID = post.PostID;
                var described = post.Described;
                var imageList = post.Image;
                var video = post.Video;
                var listImageDelete = post.ListImageDelete;
                var status = post.Status;


                bool permission = _postRepo.GetPermissionPostAction(post);
                if (!permission)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }

                result = _postService.ValidateFile(result: result, post: post);

                result = _postService.ValidateBeforeRepo(result: result, token: token, described: described, imageList: imageList, video: video);

                if (result.ResponseCode != 0)
                {
                    return result;
                }

                if (postID == null)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                _postRepo.UpdatePost(post);

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
        public ServiceResult Delete([FromForm] Post post)
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
                else if (user.Token != token)
                {

                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }

                bool permission = _postRepo.GetPermissionPostAction(post);
                if (!permission)
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
        public ServiceResult LikeStatusChanged([FromForm] React react)
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
                else if (user.Token != token)
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

                var likeCount = _postRepo.ReactPost(react);

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
        public ServiceResult ReportPost([FromForm] Report report)
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
                else if (user.Token != token)
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

                bool postExist = _postRepo.CheckPostExist(postID);
                if (!postExist)
                {
                    result.ResponseCode = 9992;
                    result.Message = "Bài viết không tồn tại";
                    return result;
                }

                _postRepo.ReportPost(report);

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
        /// Lấy tất cả Code
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_list_post")]
        public virtual ServiceResult GetListPost([FromForm] Post post)
        {
            ServiceResult result = new ServiceResult();
            try
            {

                var token = post.Token;
                var latestPostID = post.last_id;
                var pageCount = post.PageCount;
                var pageIndex = post.PageIndex;

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }
                else if (user.Token != token)
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

                var postResult = new List<Post>();
                var postMedia = new List<MediaPost>();

                var listPost = _postRepo.GetListPost(post, out postResult, out postMedia);

                if (postResult.Count <= 0)
                {
                    result.ResponseCode = 9992;
                    result.Message = "Bài viết khồng tồn tại";
                    return result;
                }

                var listDisplayPost = new List<object>();

                foreach (var item in postResult)
                {
                    string path = "C:\\Users\\DUONG.PH187315\\Desktop\\Pic\\";

                    List<Image> imageList = new List<Image>();
                    List<string> videoList = new List<string>();


                    foreach (var media in postMedia)
                    {
                        if (item.PostID == media.PostID)
                        {
                            if ((bool)media.IsImage)
                            {
                                int index = 0;
                                byte[] bytes = Convert.FromBase64String(media.Image);

                                try
                                {
                                    using (MemoryStream ms = new MemoryStream(bytes))
                                    {
                                        var image = Image.FromStream(ms);


                                        using (Bitmap bm2 = new Bitmap(ms))
                                        {
                                            bm2.Save(path + "PostID_" + item.PostID + "_Image_" + index + ".jpg");
                                        }

                                        imageList.Add(image);

                                    }
                                }
                                catch (Exception ex)
                                {
                                    result.OnException(ex);
                                }
                                index++;
                            }
                            else
                            {
                                try
                                {
                                    FileInfo video = new FileInfo(path + "PostID_" + item.PostID + "_Video.mp4");
                                    byte[] bytes = Convert.FromBase64String(media.Video);

                                    using (Stream sw = video.OpenWrite())
                                    {
                                        sw.Write(bytes, 0, bytes.Length);
                                        sw.Close();
                                    }

                                    videoList.Add(video.FullName);
                                }
                                catch (Exception ex)
                                {
                                    result.OnException(ex);
                                }
                            }
                        }
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
                        Image = imageList,
                        Video = videoList,
                        Author = new
                        {
                            AuthorID = item.Author_id,
                            AuthorName = item.Author_name,
                            AuthorAvatar = item.Author_avatar,
                        },
                        Is_blocked = item.Is_blocked,

                    };

                    listDisplayPost.Add(temp);
                }

                result.ResponseCode = 1000;
                result.Data = new
                {
                    posts = listDisplayPost,
                    NewItems = postResult[0].NewItems,
                    LastID = postResult[0].PostID,

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
        public ServiceResult GetNewListPost([FromForm] Post post)
        {

            ServiceResult result = new ServiceResult();
            try
            {
                var token = post.Token;
                var latestPostID = post.last_id;

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }
                else if (user.Token != token)
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

                var postResult = new List<Post>();
                var postMedia = new List<MediaPost>();

                var listPost = _postRepo.GetNewListPost(post, out postResult, out postMedia);

                if (postResult.Count <= 0)
                {
                    result.ResponseCode = 9992;
                    result.Message = "Bài viết khồng tồn tại";
                    return result;
                }

                var listDisplayPost = new List<object>();

                foreach (var item in postResult)
                {
                    string path = "C:\\Users\\DUONG.PH187315\\Desktop\\Pic\\";

                    List<Image> imageList = new List<Image>();
                    List<string> videoList = new List<string>();


                    foreach (var media in postMedia)
                    {
                        if (item.PostID == media.PostID)
                        {
                            if ((bool)media.IsImage)
                            {
                                int index = 0;
                                byte[] bytes = Convert.FromBase64String(media.Image);

                                try
                                {
                                    using (MemoryStream ms = new MemoryStream(bytes))
                                    {
                                        var image = Image.FromStream(ms);


                                        using (Bitmap bm2 = new Bitmap(ms))
                                        {
                                            bm2.Save(path + "PostID_" + item.PostID + "_Image_" + index + ".jpg");
                                        }

                                        imageList.Add(image);

                                    }
                                }
                                catch (Exception ex)
                                {
                                    result.OnException(ex);
                                }
                                index++;
                            }
                            else
                            {
                                try
                                {
                                    FileInfo video = new FileInfo(path + "PostID_" + item.PostID + "_Video.mp4");
                                    byte[] bytes = Convert.FromBase64String(media.Video);

                                    using (Stream sw = video.OpenWrite())
                                    {
                                        sw.Write(bytes, 0, bytes.Length);
                                        sw.Close();
                                    }

                                    videoList.Add(video.FullName);
                                }
                                catch (Exception ex)
                                {
                                    result.OnException(ex);
                                }
                            }
                        }
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
                        Image = imageList,
                        Video = videoList,
                        Author = new
                        {
                            AuthorID = item.Author_id,
                            AuthorName = item.Author_name,
                            AuthorAvatar = item.Author_avatar,
                        },
                        Is_blocked = item.Is_blocked,

                    };

                    listDisplayPost.Add(temp);
                }

                result.ResponseCode = 1000;
                result.Data = new
                {
                    //posts = listDisplayPost,
                    NewItems = postResult[0].NewItems,
                    //LastID = postResult[0].PostID,

                };
                result.Message = "OK";
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }

    }
}
