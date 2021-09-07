using CutMp3.Domain.Models;
using System.Threading.Tasks;

namespace CutMp3.Domain
{
    public interface IMp3Cutter
    {
        public string CutMp3(string mp3Path, PlayList playlist);
    }
}
