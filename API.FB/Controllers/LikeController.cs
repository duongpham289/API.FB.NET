using CNWTTBL.Entities;
using CNWTTBL.Interfaces.Repositories;
using CNWTTBL.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CNWTT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : HUSTBaseController<Like>
    {
        ILikeService _likeService;
        ILikeRepo _likeRepo;

        public LikeController(ILikeService likeService, ILikeRepo likeRepo) : base(likeService, likeRepo)
        {
            _likeService = likeService;
            _likeRepo = likeRepo;
        }


        [HttpPost("react/{postId}")]
        public IActionResult LikeStatusChanged(Guid userID, Guid postID, int status)
        {
            var res = 0;
            Like like = new Like()
            {
                PostID = postID,
                UserID = userID,
            };
            // unlike
            if (status == 0)
            {
                res = _likeRepo.DeleteCustom(userID, postID);
            }
            else
            {
                res = _likeService.InsertService(like);
            }

            return Ok(res);

        }
    }
}
