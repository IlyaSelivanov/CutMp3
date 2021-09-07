using System.Collections.Generic;

namespace CutMp3.Domain.Models
{
    public class PlayList
    {
        public List<Track> Tracks {  get; set; } = new List<Track>();
    }
}
