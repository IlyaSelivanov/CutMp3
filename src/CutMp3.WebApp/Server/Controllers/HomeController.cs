using CutMp3.Domain;
using CutMp3.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CutMp3.WebApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IDownloader _downloader;

        public HomeController(IDownloader downloader)
        {
            _downloader = downloader;
        }

        [HttpGet("int")]
        public IActionResult GetDownloadSettings()
        {
            return Ok("qwe");
        }

        [HttpPost("downloadsettings")]
        public async Task<IActionResult> DownloadSettings(DownloadSettings settings)
        {
            string path = await _downloader.DownloadUrl(settings.Url);
            return Ok();
        }
    }
}
