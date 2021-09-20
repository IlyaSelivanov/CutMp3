using CutMp3.Application;
using CutMp3.Domain.Models;
using System;
using System.Threading.Tasks;

namespace CutMp3.UIConsole
{
	static class Program
    {
        public static string playlistPath = @"C:\Users\Ilya\Downloads\LUKHASH - Better Than Reality.txt";
        public static string mp3Path = @"C:\Users\Ilya\Downloads\LUKHASH - Better Than Reality.mp3";

        static async Task Main(string[] args)
        {
            TxtPlayListParser parser = new TxtPlayListParser();
            PlayList playList = await parser.ParsePlayList(playlistPath);
            NAudioMp3Cutter cutter = new NAudioMp3Cutter();
            Console.WriteLine($"{cutter.CutMp3(mp3Path, playList)} completed");
        }
    }
}
