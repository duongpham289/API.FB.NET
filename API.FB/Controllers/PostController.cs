using CNWTTBL.Entities;
using CNWTTBL.Interfaces.Repositories;
using CNWTTBL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CNWTT.Controllers
{
    [Route("api/fb/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        IPostService _postService;
        IPostRepo _postRepo;

        public PostController(IPostService postService, IPostRepo postRepo) 
        {
            _postService = postService;
            _postRepo = postRepo;
        }

        /// <summary>
        /// Lấy tất cả Code
        /// </summary>
        /// <returns></returns>
        ///  CreatedBy: PHDUONG(27/08/2021)
        [HttpGet("getListPost/{userID}")]
        public virtual IActionResult GetListPost(Guid userID)
        {
            try
            {
                var entityCode = _postRepo.GetListPost(userID);

                if (entityCode != null)
                {
                    return StatusCode(200, entityCode);

                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = "Có lỗi xấy ra vui lòng liên hệ  để được hỗ trợ",
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
        }

        /// <summary>
        /// Lấy tất cả Code
        /// </summary>
        /// <returns></returns>
        ///  CreatedBy: PHDUONG(27/08/2021)
        [HttpGet("getNewPost/{userID}/{newestPostID}")]
        public  IActionResult GetNewListPost(Guid userID, int newestPostID)
        {
            try
            {
                var entityCode = _postRepo.GetNewListPost(userID, newestPostID);

                if (entityCode != null)
                {
                    return StatusCode(200, entityCode);

                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = "Có lỗi xấy ra vui lòng liên hệ  để được hỗ trợ",
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
        }
        /// <summary>
        /// Xử lí thêm mới dữ liệu
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        // POST api/<MISABaseController>
        [HttpPost("post")]
        public IActionResult Post([FromBody] Post entity)
        {
            
                var res = _postService.InsertPost(entity);
                return StatusCode(201, res);
            
        }

        /// <summary>
        /// Xử lí sửa đối tượng 
        /// </summary>
        /// <param name="entityId"> Id của đôi tượng </param>
        /// <param name="entity"> Dữ liệu mới </param>
        /// <returns></returns>
        // PUT api/<MISABaseController>/5
        [HttpPut("edit_post")]
        public virtual IActionResult Put(Guid entityId, [FromBody] Post entity)
        {
            
                var res = _postService.UpdatePost(entityId, entity);
                return StatusCode(200, res);
            
        }

        /// <summary>
        /// Xử lí xóa đối tượng theo Id
        /// </summary>
        /// <param name="entityId"> Id của đối tượng </param>
        /// <returns></returns>
        [HttpDelete("delete_post")]
        public virtual IActionResult Delete(Guid entityId)
        {
            
                var res = _postService.DeletePost(entityId);
                return StatusCode(200, res);
            
        }

        [HttpGet("like")]
        public IActionResult Like(Guid postID)
        {
            var res = _postService.Like(postID);
            return StatusCode(200, res);

        }
    }
}
