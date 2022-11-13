using API.FB.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CNWTT.Controllers
{
    [Route("fb")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        IReportService _reportService;

        public ReportController(IReportService reportService) 
        {
            _reportService = reportService;

        }

        [HttpGet("report")]
        public IActionResult ReportPost([FromQuery] Guid postId)
        {
            
                var res = _reportService.ReportPost(postId);
                return Ok(res);
            
        }
    }
}
