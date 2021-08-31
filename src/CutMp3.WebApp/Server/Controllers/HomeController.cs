using CutMp3.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CutMp3.WebApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("int")]
        public IActionResult GetDownloadSettings()
        {
            return Ok("qwe");
        }

        [HttpPost("downloadsettings")]
        public IActionResult DownloadSettings(DownloadSettings settings)
        {
            return Ok(settings);
        }
    }
}
