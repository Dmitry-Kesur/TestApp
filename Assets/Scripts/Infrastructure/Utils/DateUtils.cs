using System;

namespace Infrastructure.Utils
{
    public class DateUtils
    {
        public static int UtcToInt(DateTime utc) =>
            utc.Year * 10000 + utc.Month * 100 + utc.Day;
        
        public static int TodayUtcInt() =>
            UtcToInt(DateTime.UtcNow);

        public static int YesterdayUtcInt() =>
            UtcToInt(DateTime.UtcNow.AddDays(-1));
    }
}