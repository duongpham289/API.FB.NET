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

        public CommentController(ICommentRepo commentRepo, ICommentService commentService)
        {
            _commentRepo = commentRepo;
            _commentService = commentService;
        }

        [HttpGet("getComment")]
        public IActionResult Get([FromQuery]Guid postId)
        {
            try
            {
                var serviceResult = _commentRepo.GetByPostId(postId);

                if (serviceResult.Count > 0)
                    return StatusCode(1000, serviceResult);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = API.FB.Core.Resources.ResourceVN.ExceptionError_Msg,
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
            
        }

        [HttpPut("edit_comment")]
        public ServiceResult Put([FromQuery]Comment comment)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if (String.IsNullOrWhiteSpace(comment.CommentContent) || comment.UserID == Guid.Empty || comment.PostID == Guid.Empty)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                if (comment.CommentContent.Length > 65.535)
                {
                    result.ResponseCode = 1006;
                    result.Message = "Độ dài đầu vào quá mức cho phép";
                    return result;
                }
                _commentService.editComment(comment);

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
