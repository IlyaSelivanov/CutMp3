using CutMp3.Application;
using MediaToolkit;
using MediaToolkit.Model;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VideoLibrary;

namespace CutMp3.UIConsole
{
    static class Program
    {
        public static string playlistPath = @"C:\Users\Ilya\Downloads\Ashnaia Project - Escape From Reality.txt";
        public static string mp3Path = @"C:\Users\Ilya\Downloads\Ashnaia Project - Escape From Reality.mp3";

        static async Task Main(string[] args)
        {
            var playlist = await ParseTxtPlaylist(playlistPath);

            var dir = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(mp3Path), Path.GetFileNameWithoutExtension(mp3Path)));
            using (FileStream SourceStream = File.Open(mp3Path, FileMode.Open))
            {
                var mp3File = new StreamMediaFoundationReader(SourceStream);
                foreach (var track in playlist)
                {
                    var trimmed = new OffsetSampleProvider(mp3File.ToSampleProvider())
                    {
                        SkipOver = track.Start,
                        Take = track.Duration == TimeSpan.MinValue ? mp3File.TotalTime.Subtract(track.Start) : track.Duration
                    };
                    var newPath = Path.Combine(dir.FullName, track.Title + ".mp3");
                    WaveFileWriter.CreateWaveFile16(newPath, trimmed);
                    mp3File.Position = 0;
                }
            }
        }

        private static async Task<List<Track>> ParseTxtPlaylist(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string pattern = @"( )?([0-9]{1,2}:)?[0-9]{1,2}:[0-9]{1,2}";
                List<string> lines = new List<string>();

                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                    lines.Add(line);

                List<TempList> tempList = new List<TempList>();
                foreach (var l in lines)
                {
                    Match m = Regex.Match(l, pattern);
                    if (m.Success)
                        tempList.Add(new() { Title = l.Replace(m.Value, ""), Start = ParseTime(m.Value) });
                }

                List<Track> tracks = new List<Track>();
                for (int i = 0; i < tempList.Count; i++)
                {
                    Track track = new Track();

                    track.Title = tempList[i].Title;
                    track.Start = tempList[i].Start;
                    if (i == tempList.Count - 1)
                        track.Duration = TimeSpan.MinValue;
                    else
                        track.Duration = tempList[i + 1].Start.Subtract(tempList[i].Start);

                    tracks.Add(track);
                }

                return tracks;
            }
        }

        private static TimeSpan ParseTime(string time)
        {
            if (string.IsNullOrEmpty(time))
                return TimeSpan.MinValue;

            var timeArray = time.Split(":");
            if (timeArray.Length < 2)
                throw new ArgumentException("Not valid time string");

            switch (timeArray.Length)
            {
                case 2:
                    return new TimeSpan(hours: 0,
                        minutes: int.Parse(timeArray[0]),
                        seconds: int.Parse(timeArray[1]));
                case 3:
                    return new TimeSpan(hours: int.Parse(timeArray[0]),
                        minutes: int.Parse(timeArray[1]),
                        seconds: int.Parse(timeArray[2]));
                default: throw new ArgumentException("Not valid time string");
            }
        }
    }

    public class TempList
    {
        public string Title { get; set; }
        public TimeSpan Start { get; set; }
    }

    public class Track
    {
        public string Title { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
