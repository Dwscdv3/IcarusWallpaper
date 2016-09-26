using System;
using System . Collections . Generic;
using System . Windows . Threading;
using static IcarusWallpaper . Properties . Settings;
using IcarusWallpaper . Native;

namespace IcarusWallpaper
{
    static class Timer
    {
        static IDesktopWallpaper wallpaper = (IDesktopWallpaper) new DesktopWallpaper ();

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
                Default . WallpaperSetInterval = value;
                wallpaperSetTimer . Interval = value;
                if ( value == TimeSpan . Zero )
                {
                    wallpaperSetTimer . IsEnabled = false;
                }
                else
                {
                    wallpaperSetTimer . IsEnabled = true;
                }
            }
        }

        static DateTime lastFetchTime = DateTime . MinValue;
        static DateTime lastWallpaperSetTime = DateTime . MinValue;
        static DispatcherTimer fetchTimer = new DispatcherTimer ();
        static DispatcherTimer wallpaperSetTimer = new DispatcherTimer ();

        static Timer ()
        {
            fetchTimer . Tick += FetchTimer_Tick;
            wallpaperSetTimer . Interval = Default . WallpaperSetInterval;
            wallpaperSetTimer . Tick += WallpaperSetTimer_Tick;
        }

        static int wallpaperIndex = -1;
        static Random r = new Random ();
        public static List<string> Wallpapers = new List<string> ();
        private static void WallpaperSetTimer_Tick ( object sender , EventArgs e )
        {
            if ( Wallpapers . Count > 0 )
            {
                if ( Default . RandomWallpaper )
                {
                    wallpaperIndex = r . Next ( Wallpapers . Count );
                }
                else
                {
                    if (wallpaperIndex + 1 == Wallpapers.Count)
                    {
                        wallpaperIndex = 0;
                    }
                    else
                    {
                        wallpaperIndex++;
                    }
                }

                wallpaper . SetWallpaper ( null , Wallpapers [ wallpaperIndex ] );
            }
        }

        private static async void FetchTimer_Tick ( object sender , EventArgs e )
        {
            fetchTimer . Stop ();
            await Fetcher . Fetch ();
        }

        public static void ResetFetchTimer ()
        {
            fetchTimer . IsEnabled = false;
            fetchTimer . IsEnabled = true;
        }
    }
}