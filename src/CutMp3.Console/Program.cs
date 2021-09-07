using CutMp3.Application;
using CutMp3.Domain.Models;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CutMp3.UIConsole
{
    static class Program
    {
        public static string playlistPath = @"C:\Users\Ilya\Downloads\Dreaming Cooper - Parallels.txt";
        public static string mp3Path = @"C:\Users\Ilya\Downloads\Dreaming Cooper - Parallels.mp3";

        static async Task Main(string[] args)
        {
            TxtPlayListParser parser = new TxtPlayListParser();
            PlayList playList = await parser.ParsePlayList(playlistPath);
            NAudioMp3Cutter cutter = new NAudioMp3Cutter();
            Console.WriteLine($"{cutter.CutMp3(mp3Path, playList)} completed");
        }
    }
}
