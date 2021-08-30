using CutMp3.Domain;
using System.IO;
using System.Threading.Tasks;
using VideoLibrary;

namespace CutMp3.Application
{
    public class YoutubeDownloader : IDownloader
    {
        private YouTube _youtube;
        public string DownloadFolder { get; private set; }

        public YoutubeDownloader(string downloadFolder)
        {
            DownloadFolder = downloadFolder;
            _youtube = YouTube.Default;
        }

        public async Task<string> DownloadUrl(string url)
        {
            var vid = await _youtube.GetVideoAsync(url);
            string path = DownloadFolder + vid.FullName;
            await File.WriteAllBytesAsync(path, vid.GetBytes());

            return path;
        }
    }
}
