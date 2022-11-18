using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Entities;
using API.FB.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CNWTT.Controllers
{
    [Route("fb")]
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
        [HttpGet("getListPost")]
        public virtual IActionResult GetListPost([FromQuery]Guid userID)
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
        [HttpGet("getNewPost")]
        public  IActionResult GetNewListPost([FromQuery] Guid userID, [FromQuery]int newestPostID)
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
        public ServiceResult Post([FromQuery] Post post)
        {

            ServiceResult result = new ServiceResult();
            try
            {
                if (String.IsNullOrWhiteSpace(post.Content) || post.UserID == Guid.Empty)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                if (post.Content.Length > 65.535 || post.Media.Length > 65.535)
                {
                    result.ResponseCode = 1006;
                    result.Message = "Độ dài đầu vào quá mức cho phép";
                    return result;
                }
                post.PostID = Guid.NewGuid();
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
                if (String.IsNullOrWhiteSpace(post.Content) || post.UserID == Guid.Empty || post.PostID == Guid.Empty)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                if (post.Content.Length > 65.535 || post.Media.Length > 65.535)
                {
                    result.ResponseCode = 1006;
                    result.Message = "Độ dài đầu vào quá mức cho phép";
                    return result;
                }
                var postID = post.PostID;
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
        public ServiceResult Delete([FromQuery] Guid postID)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var serviceResult = _postRepo.DeletePost(postID);
                
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


        [HttpPost("react")]
        public ServiceResult LikeStatusChanged([FromQuery] React react)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if (react.UserID == Guid.Empty || react.PostId == Guid.Empty )
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                if (react.Status == null)
                {
                    react.Status = 0;
                }
                react.ReactID = Guid.NewGuid();
                _postService.Like(react);

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


        [HttpGet("report")]
        public ServiceResult ReportPost([FromQuery] Report report)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if (String.IsNullOrWhiteSpace(report.Details) || report.UserID == Guid.Empty || report.PostID == Guid.Empty)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }

                if (report.Subject.Length > 255 || report.Details.Length > 65.535)
                {
                    result.ResponseCode = 1006;
                    result.Message = "Độ dài đầu vào quá mức cho phép";
                    return result;
                }
                report.ReportID = Guid.NewGuid();
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
