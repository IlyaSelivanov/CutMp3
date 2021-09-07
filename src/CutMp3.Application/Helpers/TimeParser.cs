using System;

namespace CutMp3.Application.Helpers
{
    public static class TimeParser
    {
        public static TimeSpan ParseTime(string time)
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
}
