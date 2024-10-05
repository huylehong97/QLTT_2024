using Microsoft.Extensions.Configuration;

namespace QLBH.Business.Common;

public class ConfigConstants
{
    public readonly static int CacheHours = 4;
    public readonly static int CacheMinutes = 10;
    public readonly static int CacheMinuteNull = 1;

    #region Cache
    public static DateTime CacheHourTime(int hours = 0)
    {
        return CacheTime(hours: hours > 0 ? hours : CacheHours);
    }
    public static DateTime CacheMinuteTime(int minutes = 0)
    {
        return CacheTime(minutes: minutes > 0 ? minutes : CacheMinutes);
    }

    public static DateTime CacheNullTime(int minutes = 0)
    {
        return CacheTime(minutes: minutes > 0 ? minutes : CacheMinuteNull);
    }

    public static DateTime CacheDefaultTime()
    {
        return DateTime.Now.AddMinutes(CacheMinutes);
    }

    public static DateTime CacheTime(int minutes = 0, int hours = 0, int days = 0, int seconds = 0)
    {
        return DateTime.Now.AddDays(days).AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
    }
    #endregion
}
