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

        public HomeController(
            IDownloader downloader,
            IConverter converter,
            IHubContext<DownloadHub> hubContext,
            IBackgroundTaskQueue taskQueue)
        {
            _downloader = downloader;
            _converter = converter;
            _hubContext = hubContext;
            _taskQueue = taskQueue;
        }

        [HttpGet("int")]
        public IActionResult GetDownloadSettings()
        {
            _taskQueue.QueueBackgroundWorkItemAsync(BuildWorkItem);
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

        private async ValueTask BuildWorkItem(CancellationToken token)
        {
            // Simulate three 5-second tasks to complete
            // for each enqueued work item

            int delayLoop = 0;
            var guid = Guid.NewGuid().ToString();

            Console.WriteLine($"Queued Background Task {guid} is starting.");
            //Console.WriteLine("Queued Background Task is starting.");

            while (!token.IsCancellationRequested && delayLoop < 3)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), token);
                }
                catch (OperationCanceledException)
                {
                    // Prevent throwing if the Delay is cancelled
                }

                delayLoop++;

                Console.WriteLine("Queued Background Task is running.");
            }

            if (delayLoop == 3)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "user", $"File loaded at: {DateTime.Now}");
                Console.WriteLine("Queued Background Task is complete.");
            }
            else
            {
                Console.WriteLine("Queued Background Task was cancelled.");
            }
        }
    }
}
