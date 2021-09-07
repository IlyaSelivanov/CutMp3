using CutMp3.Domain.Models;
using System.Threading.Tasks;

namespace CutMp3.Domain
{
    public interface IPlayListParser
    {
        public Task<PlayList> ParsePlayList(string path);
    }
}
