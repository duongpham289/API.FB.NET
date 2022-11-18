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
        [HttpGet("get_list_post")]
        public virtual ServiceResult GetListPost([FromQuery] string token, Guid userID, Guid lastedPostID, int skip, int take)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if (String.IsNullOrWhiteSpace(token) || userID == Guid.Empty)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }
                var list = _postRepo.GetListPost(token, userID, lastedPostID, skip, take);

                result.ResponseCode = 1000;
                result.Data = list;
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
        ///  CreatedBy: PHDUONG(27/08/2021)
        [HttpGet("get_new_post")]
        public ServiceResult GetNewListPost([FromQuery] string token, Guid lastedPostID)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if (String.IsNullOrWhiteSpace(token) || lastedPostID == Guid.Empty)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }
                var list = _postRepo.GetNewListPost(token, lastedPostID);
                result.ResponseCode = 1000;
                result.Data = list;
                result.Message = "OK";
            }
            catch (Exception ex)
            {
                result.OnException(ex);
            }
            return result;

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

        /// <summary>
        /// Controller like
        /// </summary>
        /// <param name="react"></param>
        /// <returns></returns>
        [HttpPost("like")]
        public ServiceResult LikeStatusChanged([FromQuery] string token, Guid postID)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if (String.IsNullOrWhiteSpace(token) || postID == Guid.Empty)
                {
                    result.ResponseCode = 1002;
                    result.Message = "Số lượng Parameter không đầy đủ";
                    return result;
                }
                var res = _postRepo.LikePost(token, postID);
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
