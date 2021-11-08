using CutMp3.Domain;
using CutMp3.Domain.Enums;
using CutMp3.Domain.Models;
using CutMp3.WebApp.Server.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
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
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IDownloader downloader,
            IConverter converter,
            IHubContext<DownloadHub> hubContext,
            IBackgroundTaskQueue taskQueue,
            ILogger<HomeController> logger)
        {
            _downloader = downloader;
            _converter = converter;
            _hubContext = hubContext;
            _taskQueue = taskQueue;
            _logger = logger;
        }

        [HttpPost("downloadsettings")]
        public async Task<IActionResult> DownloadSettings(DownloadSettings settings)
        {
            await _taskQueue.QueueBackgroundWorkItemAsync(
                new Tuple<DownloadSettings, Func<CancellationToken, DownloadSettings, ValueTask>>(settings, BuildWorkItem));

            return Ok();
        }

        private async ValueTask BuildWorkItem(CancellationToken token, DownloadSettings settings)
        {
            var guid = Guid.NewGuid().ToString();

            _logger.LogInformation($"Queued Background Task {guid} is starting.");
            _logger.LogInformation($"Downloading...");
            string path = await _downloader.DownloadUrl(settings.Url);

            await Task.Run(() =>
            {
                if (settings.Extension == MediaExtensions.Mp3.Extensions)
                {
                    _logger.LogInformation($"Converting...");
                    _converter.ConvertMp4ToMp3(path);

                    _logger.LogInformation($"Cleanning...");
                    System.IO.File.Delete(path);
                }
            });

            _logger.LogInformation($"SignalR hub notifying...");
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "user", $"File loaded at: {DateTime.Now}");

            _logger.LogInformation($"Queued Background Task {guid} is complete.");
        }
    }
}
