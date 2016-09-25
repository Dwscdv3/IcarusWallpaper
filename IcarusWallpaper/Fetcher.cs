﻿using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Diagnostics;
using System . IO;
using System . Linq;
using System . Net;
using System . Text;
using System . Text . RegularExpressions;
using System . Threading . Tasks;
using static IcarusWallpaper . Parameters;

namespace IcarusWallpaper
{
    static class Fetcher
    {
        static MainWindow window;

        static bool _isFetching = false;
        public static bool IsFetching
        {
            get
            {
                return _isFetching;
            }
            set
            {
                _isFetching = value;
                window . buttonManualFetch . IsEnabled = !value;
            }
        }

        public static void BindWindow ( MainWindow window )
        {
            Fetcher . window = window;
        }

        public static async Task Fetch ()
        {
            try
            {
                IsFetching = true;

                var c = new WebClient ();
                //c . DownloadDataCompleted += CheckError;

                window . label1 . Content = "Fetching RSS...";

                string url = "";
                switch ( FetchSource )
                {
                case WordPressCategory . Index:
                    url = IcarusNewestRssUrl;
                    break;
                case WordPressCategory . Yesterday:
                    url = IcarusYesterdayRssUrl;
                    break;
                default:
                    url = IcarusNewestRssUrl;
                    Debug . WriteLine ( "FetchSource is invalid." );
                    break;
                }
                var rss = Encoding . UTF8 . GetString ( await c . DownloadDataTaskAsync ( IcarusNewestRssUrl ) );
                var matches = Regex . Matches ( rss , @"http://icarus\.silversky\.moe:666/illustration/\d+" );

                int downloaded = 0, skipped = 0, actualAmount = Min ( FetchAmount , matches . Count );
                for ( int i = matches . Count - 1 ; i >= 0 ; i-- )
                {
                    var match = matches [ i ];
                    if ( FetchSource == WordPressCategory . Index && i >= FetchAmount )
                    {
                        continue;
                    }

                    window . label1 . Content = $"Fetching page {actualAmount - i} / {actualAmount} ...";

                    var html = Encoding . UTF8 . GetString ( await c . DownloadDataTaskAsync ( match . Value ) );
                    var c2 = new WebClient ();
                    //c2 . DownloadFileCompleted += CheckError;
                    c2 . DownloadProgressChanged += ( s2 , e2 ) =>
                    {
                        window . DownloadProgress . Value = e2 . BytesReceived;
                        window . DownloadProgress . Maximum = e2 . TotalBytesToReceive;
                    };

                    window . label1 . Content = $"Downloading {actualAmount - i} / {actualAmount} ...";

                    var uri = new Uri ( Regex . Match ( html , "(?<=<img.+?)http://icarus\\.silversky\\.moe:666/wp-content/uploads/\\d{4}/\\d{2}/.+?(.jpg|.png)" ) . Value );
                    var path = DownloadPath + Regex . Match ( html , "(?<=(?<=<img.+?)http://icarus\\.silversky\\.moe:666/wp-content/uploads/\\d{4}/\\d{2}/).+?(.jpg|.png)" ) . Value;
                    var req = WebRequest . CreateHttp ( uri );
                    var res = req . GetResponse ();
                    var length = res . ContentLength;
                    res . Close ();
                    if ( !File . Exists ( path ) || new FileInfo ( path ) . Length != length )
                    {
                        await c2 . DownloadFileTaskAsync ( uri , path );
                        downloaded++;
                    }
                    else
                    {
                        skipped++;
                    }
                    window . DownloadProgress . Value = 0;
                }
                window . label1 . Content = $"{DateTime . Now . ToShortTimeString ()}  Done. {downloaded} downloaded, {skipped} skipped.";
                IsFetching = false;
            }
            catch ( Exception ex )
            {
                Debug . WriteLine ( ex . Message );
                window . label1 . Content = $"{DateTime . Now . ToShortTimeString ()}  Aborted due to a network error. (1)";
                IsFetching = false;
            }
        }

        //static void CheckError ( object sender , AsyncCompletedEventArgs e )
        //{
        //    Debug . Write ( "Cancelled: " + ( e . Cancelled ? "true" : "false" ) );
        //    Debug . Write ( ", Error: " );
        //    Debug . WriteLine ( e . Error?.Message );
        //    label1 . Content = $"{DateTime . Now . ToShortTimeString ()}  Aborted due to a network error. (2)";
        //}

        public static int Min ( int a , int b )
        {
            return a < b ? a : b;
        }
    }
}
