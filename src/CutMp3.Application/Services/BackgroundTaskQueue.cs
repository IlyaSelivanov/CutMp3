using CutMp3.Domain;
using CutMp3.Domain.Models;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CutMp3.Application.Services
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Tuple<DownloadSettings, Func<CancellationToken, DownloadSettings, ValueTask>>> _queue;

        public BackgroundTaskQueue(int capacity)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<Tuple<DownloadSettings, Func<CancellationToken, DownloadSettings, ValueTask>>>(options);
        }

        public async ValueTask QueueBackgroundWorkItemAsync(
            Tuple<DownloadSettings, Func<CancellationToken, DownloadSettings, ValueTask>> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask<Tuple<DownloadSettings, Func<CancellationToken, DownloadSettings, ValueTask>>> DequeueAsync(
            CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);

            return workItem;
        }
    }
}
