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

                if (!String.IsNullOrWhiteSpace(result.code))
                {
                    return result;
                }

                var postID = _postRepo.InsertPost(post);

                result.code = "1000";
                result.message = "OK";
                result.data = new { id = postID.ToString() };

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

                if (!String.IsNullOrWhiteSpace(result.code))
                {
                    return result;
                }

                var postResult = new List<Post>();
                var postMedia = new List<MediaPost>();

                var res = _postRepo.GetPost(post, out postResult, out postMedia);

                if (postResult.Count <= 0)
                {
                    result.code = "9992";
                    result.message = "Post is not existed";
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


                result.code = "1000";
                result.message = "OK";
                result.data = new
                {
                    id = postResult[0].PostID,
                    described = postResult[0].Described,
                    created = postResult[0].CreatedDate,
                    modified = postResult[0].ModifiedDate,
                    like = postResult[0].ReactCount,
                    comment = postResult[0].CommentCount,
                    is_liked = postResult[0].Is_liked,
                    image = imageList,
                    video = videoList,
                    author = new
                    {
                        authorid = postResult[0].Author_id,
                        authorname = postResult[0].Author_name,
                        authoravatar = postResult[0].Author_avatar,
                    },
                    is_blocked = postResult[0].Is_blocked,

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
                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }

                result = _postService.ValidateFile(result: result, post: post);

                result = _postService.ValidateBeforeRepo(result: result, token: token, described: described, imageList: imageList, video: video);

                if (String.IsNullOrWhiteSpace(result.code))
                {
                    return result;
                }

                if (postID == null)
                {
                    result.code = "1002";
                    result.message = "Parameter is not enough";
                    return result;
                }

                _postRepo.UpdatePost(post);

                result.code = "1000";
                result.message = "OK";
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
                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }
                else if (user.Token != token)
                {

                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }

                bool permission = _postRepo.GetPermissionPostAction(post);
                if (!permission)
                {
                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }

                if (postID == null)
                {
                    result.code = "1002";
                    result.message = "Parameter is not enough";
                    return result;
                }


                var serviceResult = _postRepo.DeletePost(post);

                result.code = "1000";
                result.message = "OK";
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
        [HttpPost("like")]
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
                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }
                else if (user.Token != token)
                {

                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }

                if (String.IsNullOrWhiteSpace(postID))
                {
                    result.code = "1002";
                    result.message = "Parameter is not enough";
                    return result;
                }

                var likeCount = _postRepo.ReactPost(react);

                result.code = "1000";
                result.message = "OK";
                result.data = new
                {
                    like = likeCount
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
                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }
                else if (user.Token != token)
                {

                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }

                if (String.IsNullOrWhiteSpace(details) || String.IsNullOrWhiteSpace(subject) || postID == null)
                {
                    result.code = "1002";
                    result.message = "Parameter is not enough";
                    return result;
                }

                if (subject.Length > 255 || details.Length > 65.535)
                {
                    result.code = "1006";
                    result.message = "Parameter value is invalid";
                    return result;
                }

                bool postExist = _postRepo.CheckPostExist(postID);
                if (!postExist)
                {
                    result.code = "9992";
                    result.message = "Post is not existed";
                    return result;
                }

                _postRepo.ReportPost(report);

                result.code = "1000";
                result.message = "OK";
                return result;
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }

        /// <summary>
        /// Lấy tất cả code
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
                var pageCount = post.count;
                var pageIndex = post.index;

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }
                else if (user.Token != token)
                {

                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }

                if (latestPostID == null || pageCount == null || pageIndex == null)
                {
                    result.code = "1002";
                    result.message = "Parameter is not enough";
                    return result;
                }

                var postResult = new List<Post>();
                var postMedia = new List<MediaPost>();

                var listPost = _postRepo.GetListPost(post, out postResult, out postMedia);

                if (postResult.Count <= 0)
                {
                    result.code = "9992";
                    result.message = "Post is not existed";
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
                        id = item.PostID,
                        described = item.Described,
                        created = item.CreatedDate,
                        modified = item.ModifiedDate,
                        like = item.ReactCount,
                        comment = item.CommentCount,
                        is_liked = (bool)item.Is_liked ? "1" : "0",
                        image = imageList,
                        video = videoList,
                        author = new
                        {
                            id = item.Author_id,
                            username = item.Author_name,
                            avatar = item.Author_avatar,
                        },
                        is_blocked = (bool)item.Is_blocked ? "1" : "0",

                    };

                    listDisplayPost.Add(temp);
                }

                result.code = "1000";
                result.data = new
                {
                    posts = listDisplayPost,
                    NewItems = postResult[0].NewItems,
                    LastID = postResult[0].PostID,

                };
                result.message = "OK";
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }

        /// <summary>
        /// Lấy tất cả code
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
                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }
                else if (user.Token != token)
                {
                    result.code = "1009";
                    result.message = "Not access";
                    return result;
                }

                if (latestPostID == null)
                {
                    result.code = "1002";
                    result.message = "Parameter is not enough";
                    return result;
                }

                var postResult = new List<Post>();
                var postMedia = new List<MediaPost>();

                var listPost = _postRepo.GetNewListPost(post, out postResult, out postMedia);

                if (postResult.Count <= 0)
                {
                    result.code = "9992";
                    result.message = "Post is not existed";
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
                        id = item.PostID,
                        described = item.Described,
                        created = item.CreatedDate,
                        modified = item.ModifiedDate,
                        like = item.ReactCount,
                        comment = item.CommentCount,
                        is_liked = (bool)item.Is_liked ? "1" : "0",
                        image = imageList,
                        video = videoList,
                        author = new
                        {
                            id = item.Author_id,
                            username = item.Author_name,
                            avatar = item.Author_avatar,
                        },
                        is_blocked = (bool)item.Is_blocked ? "1" : "0",

                    };

                    listDisplayPost.Add(temp);
                }

                result.code = "1000";
                result.data = new
                {
                    //posts = listDisplayPost,
                    new_items = postResult[0].NewItems.ToString(),
                    //LastID = postResult[0].PostID,

                };
                result.message = "OK";
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }

    }
}
