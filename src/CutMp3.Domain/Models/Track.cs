using System;

namespace CutMp3.Domain.Models
{
    public class Track
    {
        public string Title { get; set; } = "";
        public TimeSpan Start { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
