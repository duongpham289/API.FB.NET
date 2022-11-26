using API.FB.Core.Entities;
using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.FB.Core.Controllers
{
    [Route("fb")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        ICommentRepo _commentRepo;

        IUserRepository _userRepository;

        public CommentController(ICommentRepo commentRepo, IUserRepository userRepository)
        {
            _commentRepo = commentRepo;
            _userRepository = userRepository;
        }

        [HttpGet("get_comment")]
        public ServiceResult Get([FromForm] Comment comment)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var token = comment.Token;
                var postId = comment.PostID;
                var index = comment.PageIndex;
                var count = comment.PageSize;

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

                if (index == null || count == null)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                var listComment = _commentRepo.GetByPostId(comment);

                var listDisplayComment = new List<object>();

                if (listComment != null)
                {
                    foreach (var item in listComment)
                    {
                        var temp = new
                        {
                            CommentContent = item.CommentContent,
                            Created = item.CreatedDate,
                            Poster = new
                            {
                                UserID = item.UserID,
                                UsserName = item.FullName,
                                Avatar = item.Avatar,
                            }

                        };

                        listDisplayComment.Add(temp);
                    }


                }
                result.ResponseCode = 1000;
                result.Message = "OK";
                result.Data = new
                {
                    PostID = postId,
                    Comment = listDisplayComment
                };
                return result;
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }

        [HttpPut("set_comment")]
        public ServiceResult Put([FromForm] Comment comment)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var token = comment.Token;
                var postId = comment.PostID;
                var commentContent = comment.CommentContent;
                var index = comment.PageIndex;
                var count = comment.PageSize;

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


                if (String.IsNullOrWhiteSpace(commentContent) || comment.PostID == null)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                if (commentContent.Length > 65.535)
                {
                    result.ResponseCode = 1006;
                    result.Message = "Độ dài đầu vào quá mức cho phép";
                    return result;
                }


                var listComment = _commentRepo.InsertComment(comment);


                var listDisplayComment = new List<object>();

                foreach (var item in listComment)
                {
                    var temp = new
                    {
                        CommentContent = item.CommentContent,
                        Created = item.CreatedDate,
                        Poster = new
                        {
                            UserID = item.UserID,
                            UsserName = item.FullName,
                            Avatar = item.Avatar,
                        }

                    };

                    listDisplayComment.Add(temp);
                }


                result.ResponseCode = 1000;
                result.Message = "OK";
                result.Data = new
                {
                    PostID = postId,
                    Comment = listDisplayComment
                };
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
