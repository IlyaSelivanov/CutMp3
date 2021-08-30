using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CutMp3.Domain
{
    public interface IDownloader
    {
        Task<string> DownloadUrl(string url);
    }
}
