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
        ICommentService _commentService;

        IUserRepository _userRepository;

        public CommentController(ICommentRepo commentRepo, ICommentService commentService, IUserRepository userRepository)
        {
            _commentRepo = commentRepo;
            _commentService = commentService;
            _userRepository = userRepository;
        }

        [HttpGet("getComment")]
        public ServiceResult Get([FromQuery] Comment comment)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var token = comment.Token;
                var postId = comment.PostID;
                var index = comment.Index;
                var count = comment.Count;

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }

                if (index == null || count == null || String.IsNullOrWhiteSpace(token))
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                var serviceResult = _commentRepo.GetByPostId(comment);

            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

        }

        [HttpPut("editComment")]
        public ServiceResult Put([FromQuery]Comment comment)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var token = comment.Token;
                var postId = comment.PostID;
                var commentContent = comment.CommentContent;
                var index = comment.Index;
                var count = comment.Count;

                User user = _userRepository.GetUserByToken(token);
                if (user == null)
                {
                    result.ResponseCode = 1009;
                    result.Message = "Không có quyền truy cập tài nguyên";
                    return result;
                }


                if (String.IsNullOrWhiteSpace(commentContent) || comment.UserID == Guid.Empty || comment.PostID == null)
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
                _commentService.InsertComment(comment);

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
