using API.FB.Core.Interfaces.Services;
using CNWTTBL.Entities;
using CNWTTBL.Interfaces.Repositories;
using CNWTTBL.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CNWTT.Controllers
{
    [Route("api/fb/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        IReportService _reportService;

        public ReportController(IReportService reportService) 
        {
            _reportService = reportService;

        }

        [HttpGet("report/{postId}")]
        public IActionResult ReportPost(Guid postId)
        {
            
                var res = _reportService.ReportPost(postId);
                return Ok(res);
            
        }
    }
}
