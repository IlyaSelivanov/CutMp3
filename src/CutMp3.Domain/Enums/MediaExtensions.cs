namespace CutMp3.Domain.Enums
{
    public sealed class MediaExtensions
    {
        public string Extensions { get; private set; }
        public MediaExtensions(string extension) {  Extensions =  extension; }

        public static MediaExtensions Mp4 { get { return new MediaExtensions(".mp4"); } }
        public static MediaExtensions Mp3 { get { return new MediaExtensions(".mp3"); } }
    }
}
