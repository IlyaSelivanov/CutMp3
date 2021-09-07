using CutMp3.Domain;
using CutMp3.Domain.Enums;
using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CutMp3.Application
{
    public class MediaConverter : IConverter
    {
        public string ConvertMp4ToMp3(string path)
        {
            if(string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (Path.GetExtension(path) != MediaExtensions.Mp4.Extensions)
                throw new ArgumentException("Not valid file extension");

            var input = new MediaFile { Filename = path };
            string outputFile = Path.Combine(
                Path.GetDirectoryName(path),
                Path.GetFileName(path).Replace(".mp4", ".mp3"));
            var output = new MediaFile { Filename = Path.Combine(Path.GetDirectoryName(path), outputFile) };

            try
            {
                using (var engine = new Engine())
                {
                    engine.GetMetadata(input);

                    engine.Convert(input, output);
                }
            } catch (Exception)
            {
                throw;
            }

            return outputFile;
        }
    }
}
