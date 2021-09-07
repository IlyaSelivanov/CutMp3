using CutMp3.Application.Helpers;
using CutMp3.Domain;
using CutMp3.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CutMp3.Application
{
    public class TxtPlayListParser : IPlayListParser
    {
        public async Task<PlayList> ParsePlayList(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string pattern = @"( )?([0-9]{1,2}:)?[0-9]{1,2}:[0-9]{1,2}";

                var lines = await File.ReadAllLinesAsync(path);

                List<TmpTrackList> tempList = new List<TmpTrackList>();
                foreach (var l in lines)
                {
                    Match m = Regex.Match(l, pattern);
                    if (m.Success)
                        tempList.Add(new() { Title = l.Replace(m.Value, ""), Start = TimeParser.ParseTime(m.Value) });
                }

                PlayList playlist = new();
                for (int i = 0; i < tempList.Count; i++)
                {
                    Track track = new Track();

                    track.Title = tempList[i].Title;
                    track.Start = tempList[i].Start;
                    if (i == tempList.Count - 1)
                        track.Duration = TimeSpan.MinValue;
                    else
                        track.Duration = tempList[i + 1].Start.Subtract(tempList[i].Start);

                    playlist.Tracks.Add(track);
                }

                return playlist;
            }
        }
        private class TmpTrackList
        {
            public string Title { get; set; } = "";
            public TimeSpan Start { get; set; }
        }
    }
}
