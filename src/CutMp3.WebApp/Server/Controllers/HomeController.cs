using CutMp3.Domain;
using CutMp3.Domain.Enums;
using CutMp3.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CutMp3.WebApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IDownloader _downloader;
        private IConverter _converter;

        public HomeController(
            IDownloader downloader,
            IConverter converter)
        {
            _downloader = downloader;
            _converter = converter;
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

            if (settings.Extension == MediaExtensions.Mp3.Extensions)
            {
                await Task.Run(() =>
                {
                    _converter.ConvertMp4ToMp3(path);
                    System.IO.File.Delete(path);
                });
            }

            return Ok();
        }
    }
}
