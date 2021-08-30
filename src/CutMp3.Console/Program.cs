using CutMp3.Application;
using System;
using System.Threading.Tasks;

namespace CutMp3.UIConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //string line = "07. Sleeping Forest - Alone in the Night 47:45";
            //DownloadFromYoutube("https://www.youtube.com/watch?v=w6eyvCzlBac");

            YoutubeDownloader downloader = new YoutubeDownloader(@"C:\Users\Ilya\Downloads\");
            Console.WriteLine(await downloader.DownloadUrl("https://www.youtube.com/watch?v=XnURvcdOUvY"));
        }

        //static void DownloadFromYoutube(string url)
        //{
        //    var source = @"C:\Users\Ilya\Downloads\";
        //    var youtube = YouTube.Default;
        //    var vid = youtube.GetVideo(url);
        //    File.WriteAllBytes(source + vid.FullName, vid.GetBytes());

        //    var inputFile = new MediaFile { Filename = source + vid.FullName };
        //    var outputFile = new MediaFile { Filename = $"{source + vid.FullName}.mp3" };

        //    using (var engine = new Engine())
        //    {
        //        engine.GetMetadata(inputFile);

        //        engine.Convert(inputFile, outputFile);
        //    }
        //}
    }
}
