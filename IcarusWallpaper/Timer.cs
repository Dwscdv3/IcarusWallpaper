using System;
using System . Windows . Threading;

namespace IcarusWallpaper
{
    static class Timer
    {
        public static TimeSpan FetchInterval
        {
            get
            {
                return fetchTimer . Interval;
            }
            set
            {
                fetchTimer . Interval = value;
                if ( !fetchTimer . IsEnabled )
                {
                    fetchTimer . Start ();
                }
            }
        }
        public static TimeSpan WallpaperSetInterval
        {
            get
            {
                return wallpaperSetTimer . Interval;
            }
            set
            {
                wallpaperSetTimer . Interval = value;
                if ( !wallpaperSetTimer . IsEnabled )
                {
                    wallpaperSetTimer . Start ();
                }
            }
        }

        static DateTime lastFetchTime = DateTime . MinValue;
        static DateTime lastWallpaperSetTime = DateTime . MinValue;
        static DispatcherTimer fetchTimer = new DispatcherTimer ();
        static DispatcherTimer wallpaperSetTimer = new DispatcherTimer ();
    }
}