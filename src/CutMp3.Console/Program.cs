using MediaToolkit;
using MediaToolkit.Model;
using System.IO;
using VideoLibrary;

namespace CutMp3.UIConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = @"C:\Users\Ilya\Downloads\";
            var youtube = YouTube.Default;
            var vid = youtube.GetVideo("https://www.youtube.com/watch?v=Z1Ja2_NHEi8");
            File.WriteAllBytes(source + vid.FullName, vid.GetBytes());

            var inputFile = new MediaFile { Filename = source + vid.FullName };
            var outputFile = new MediaFile { Filename = $"{source + vid.FullName}.mp3" };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                engine.Convert(inputFile, outputFile);
            }
        }
    }
}
