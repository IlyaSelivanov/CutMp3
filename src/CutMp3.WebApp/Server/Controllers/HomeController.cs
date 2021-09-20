using CutMp3.Domain;
using CutMp3.Domain.Enums;
using CutMp3.Domain.Models;
using CutMp3.WebApp.Server.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace CutMp3.WebApp.Server.Controllers
{
	[Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IDownloader _downloader;
        private readonly IConverter _converter;
        private readonly IHubContext<DownloadHub> _hubContext;

        public HomeController(
            IDownloader downloader,
            IConverter converter,
            IHubContext<DownloadHub> hubContext)
        {
            _downloader = downloader;
            _converter = converter;
            _hubContext = hubContext;
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

            await Task.Run(() =>
            {
                if (settings.Extension == MediaExtensions.Mp3.Extensions)
                {
                    _converter.ConvertMp4ToMp3(path);
                    System.IO.File.Delete(path);
                }
            });

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "user", $"File loaded at: {DateTime.Now}");

            return Ok();
        }
    }
}
