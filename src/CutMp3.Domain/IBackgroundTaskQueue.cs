using CutMp3.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CutMp3.Domain
{
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueBackgroundWorkItemAsync(
            Tuple<DownloadSettings, Func<CancellationToken, DownloadSettings, ValueTask>> workItem);

        ValueTask<Tuple<DownloadSettings, Func<CancellationToken, DownloadSettings, ValueTask>>> DequeueAsync(
            CancellationToken cancellationToken);
    }
}
