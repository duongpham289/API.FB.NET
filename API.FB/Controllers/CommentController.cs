using CNWTTBL.Entities;
using CNWTTBL.Interfaces.Repositories;
using CNWTTBL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CNWTT.Controllers
{
    [Route("api/fb/[controller]")]
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

        [HttpGet("getComment/{postId}")]
        public IActionResult Get(Guid postId)
        {
            
                var res = _commentRepo.GetByPostId(postId);
                return Ok(res);
            
        }

        [HttpPut("edit_comment/{postId}")]
        public IActionResult Put(Guid postId)
        {
            
                var res = _commentService.editComment(postId);
                return Ok(res);
            
            
        }
    }
}
