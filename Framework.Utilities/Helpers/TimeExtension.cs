using System;

namespace Framework.Utilities.Helpers
{
    public static class TimeExtension
    {
        public static DateTime Truncate(this DateTime date, long res)
        {
            return new DateTime(date.Ticks - (date.Ticks % res), date.Kind);
        }
    }
}