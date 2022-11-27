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
        IPostRepo _postRepo;

        IUserRepository _userRepository;

        public CommentController(ICommentRepo commentRepo, IPostRepo postRepo, IUserRepository userRepository)
        {
            _commentRepo = commentRepo;
            _postRepo = postRepo;
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
                var index = comment.index;
                var count = comment.count;

                bool postExist = _postRepo.CheckPostExist(postId);
                if (!postExist)
                {
                    result.code = "9992";
                    result.message = "Post is not existed";
                    return result;
                }

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

                if (index == null || count == null)
                {
                    result.code = "1002";
                    result.message = "Parameter is not enough";
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
                            content = item.content,
                            created = item.CreatedDate,
                            poster = new
                            {
                                id = item.UserID,
                                username = item.FullName,
                                avatar = item.Avatar,
                            }

                        };

                        listDisplayComment.Add(temp);
                    }


                }
                result.code = "1000";
                result.message = "OK";
                result.data = new
                {
                    id = postId.ToString(),
                    comment = listDisplayComment
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
                var commentContent = comment.content;
                var index = comment.index;
                var count = comment.count;

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


                if (String.IsNullOrWhiteSpace(commentContent) || comment.PostID == null)
                {
                    result.code = "1002";
                    result.message = "Parameter is not enough";
                    return result;
                }

                if (commentContent.Length > 65.535)
                {
                    result.code = "1004";
                    result.message = "Parameter value is invalid";
                    return result;
                }


                var listComment = _commentRepo.InsertComment(comment);


                var listDisplayComment = new List<object>();

                foreach (var item in listComment)
                {
                    var temp = new
                    {
                        content = item.content,
                        created = item.CreatedDate,
                        poster = new
                        {
                            id = item.UserID,
                            username = item.FullName,
                            avatar = item.Avatar,
                        }

                    };

                    listDisplayComment.Add(temp);
                }


                result.code = "1000";
                result.message = "OK";
                result.data = new
                {
                    id = postId,
                    comment = listDisplayComment
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
