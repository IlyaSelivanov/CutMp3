using CutMp3.Domain;
using CutMp3.Domain.Models;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.IO;

namespace CutMp3.Application
{
    public class NAudioMp3Cutter : IMp3Cutter
    {
        public string CutMp3(string mp3Path, PlayList playlist)
        {
            if(string.IsNullOrEmpty(mp3Path))
                throw new ArgumentNullException(mp3Path);

            if (playlist == null || playlist.Tracks.Count == 0)
                throw new ArgumentNullException(mp3Path);

            var dir = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(mp3Path),
                Path.GetFileNameWithoutExtension(mp3Path)));

            using (FileStream SourceStream = File.Open(mp3Path, FileMode.Open))
            {
                using (var mp3File = new StreamMediaFoundationReader(SourceStream))
                {
                    foreach (var track in playlist.Tracks)
                    {
                        var trimmed = new OffsetSampleProvider(mp3File.ToSampleProvider())
                        {
                            SkipOver = track.Start,
                            Take = track.Duration == TimeSpan.MinValue ? mp3File.TotalTime.Subtract(track.Start) : track.Duration
                        };
                        var newPath = Path.Combine(dir.FullName, track.Title + ".wav");
                        WaveFileWriter.CreateWaveFile16(newPath, trimmed);

                        using (var reader = new WaveFileReader(newPath))
                        {
                            MediaFoundationEncoder.EncodeToMp3(reader,
                                newPath.Replace(".wav", ".mp3"), 48000);
                        }

                        mp3File.Position = 0;
                        mp3File.Flush();
                    }
                }
            }

            foreach (var file in dir.GetFiles("*.wav"))
                file.Delete();

            return dir.FullName;
        }
    }
}
