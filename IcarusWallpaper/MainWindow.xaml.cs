using System;
using System . Collections . Generic;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Documents;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Media . Imaging;
using System . Windows . Navigation;
using System . Windows . Shapes;
using System . Runtime . InteropServices;
using System . Net;
using Microsoft . Win32;
using IcarusWallpaper . Native;
using System . Net . Http;
using static IcarusWallpaper . Parameters;
using static IcarusWallpaper . Timer;
using System . Text . RegularExpressions;
using System . Diagnostics;
using System . IO;
using System . ComponentModel;

namespace IcarusWallpaper
{
    public partial class MainWindow : Window
    {
        IDesktopWallpaper wallpaper = (IDesktopWallpaper) new DesktopWallpaper ();
        
        public MainWindow ()
        {
            InitializeComponent ();
        }

        private void Window_Loaded ( object sender , RoutedEventArgs e )
        {
            DownloadPath = @"G:\Icarus\";
            
        }

        private void test_Click ( object sender , RoutedEventArgs e )
        {
            OpenFileDialog ofd = new OpenFileDialog ();
            ofd . InitialDirectory = Environment . GetFolderPath ( Environment . SpecialFolder . MyPictures );
            if ( ofd . ShowDialog ( this ) == true )
            {
                wallpaper . SetWallpaper ( null , ofd . FileName );
            }
        }

        bool _isFetching = false;
        bool isFetching
        {
            get
            {
                return _isFetching;
            }
            set
            {
                _isFetching = value;
                buttonManualFetch . IsEnabled = !value;
            }
        }
        private async void buttonManualFetch_Click ( object sender , RoutedEventArgs e )
        {
            try
            {
                isFetching = true;

                var c = new WebClient ();
                c . DownloadDataCompleted += CheckError;

                label1 . Content = "Fetching RSS...";

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
                int i = 0, downloaded = 0, skipped = 0;
                foreach ( Match match in matches )
                {
                    i++;
                    if ( FetchSource == WordPressCategory.Index && i > FetchAmount )
                    {
                        break;
                    }

                    label1 . Content = $"Fetching page {i}...";

                    var html = Encoding . UTF8 . GetString ( await c . DownloadDataTaskAsync ( match . Value ) );
                    var c2 = new WebClient ();
                    c2 . DownloadFileCompleted += CheckError;
                    c2 . DownloadProgressChanged += ( s2 , e2 ) =>
                      {
                          DownloadProgress . Value = e2 . BytesReceived;
                          DownloadProgress . Maximum = e2 . TotalBytesToReceive;
                      };

                    label1 . Content = $"Downloading {i}...";

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
                    DownloadProgress . Value = 0;
                }
                label1 . Content = $"{DateTime . Now . ToShortTimeString ()}  Done. {downloaded} downloaded, {skipped} skipped.";
                isFetching = false;
            }
            catch ( Exception ex )
            {
                Debug . WriteLine ( ex . Message );
                label1 . Content = $"{DateTime . Now . ToShortTimeString ()}  Aborted due to a network error. (1)";
                isFetching = false;
            }
        }

        void CheckError ( object sender , AsyncCompletedEventArgs e )
        {
            Debug . Write ( "Cancelled: " + ( e . Cancelled ? "true" : "false" ) );
            Debug . Write ( ", Error: " );
            Debug . WriteLine ( e . Error?.Message );
            label1 . Content = $"{DateTime . Now . ToShortTimeString ()}  Aborted due to a network error. (2)";
        }

        private void TextBox_DigitOnly ( object sender , TextCompositionEventArgs e )
        {
            foreach ( char c in e . Text )
            {
                if ( !char . IsDigit ( c ) )
                {
                    e . Handled = true;
                }
            }
        }

        private void textBoxAmount_LostFocus ( object sender , RoutedEventArgs e )
        {
            if ( textBoxAmount . Text . Length == 0 || int . Parse ( textBoxAmount . Text ) == 0 )
            {
                textBoxAmount . Text = "1";
            }
        }
    }
}
