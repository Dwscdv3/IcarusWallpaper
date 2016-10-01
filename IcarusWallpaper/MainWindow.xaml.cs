using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Diagnostics;
using System . IO;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Input;
using System . Windows . Threading;
//using static IcarusWallpaper . Fetcher;
using static IcarusWallpaper . Parameters;
using static IcarusWallpaper . Properties . Settings;
using static IcarusWallpaper . Timer;

namespace IcarusWallpaper
{
    public partial class MainWindow : Window
    {
        DispatcherTimer showWindowTimer = new DispatcherTimer ();

        public MainWindow ()
        {
            InitializeComponent ();
        }

        private async void Window_Loaded ( object sender , RoutedEventArgs e )
        {
            NotifyIcon . Create ( this );
            Fetcher . BindWindow ( this );

            textBoxAmount . Text = Default . FetchAmount . ToString ();
            FetchAmount = Default . FetchAmount;

            #region FetchSource
            switch ( Default . FetchSource )
            {
            default:
            case 0:
                sourceNewest . IsChecked = true;
                break;
            case 1:
                sourceYesterday . IsChecked = true;
                break;
            }
            #endregion

            randomCheckBox . IsChecked = Default . RandomWallpaper;
            textBoxInterval . Text = Default . WallpaperSetInterval . TotalMinutes . ToString ();
            wallpaperMainSwitch . IsChecked = Default . WallpaperMainSwitch;

            if ( !string . IsNullOrWhiteSpace ( Default . DownloadPath ) )
            {
                setPath ( Default . DownloadPath );
            }

            filterCheckBox . IsChecked = Default . FilterAspectRatio;
            ratio . Value = Default . AspectRatioLimit;
            ratioText . Text = Default . AspectRatioLimit . ToString ();

            FetchInterval = TimeSpan . FromHours ( 6 );
            if ( !string . IsNullOrWhiteSpace ( Default . DownloadPath ) )
            {
                await Fetcher . Fetch ();
            }

            showWindowTimer . Interval = TimeSpan . FromSeconds ( 1 );
            showWindowTimer . Tick += ( s2 , e2 ) =>
            {
                if ( File . Exists ( AppDomain . CurrentDomain . BaseDirectory + "Icarus_Show" ) )
                {
                    Show ();
                    Activate ();
                    File . Delete ( AppDomain . CurrentDomain . BaseDirectory + "Icarus_Show" );
                }
            };
            showWindowTimer . Start ();
        }

        //private void test_Click ( object sender , RoutedEventArgs e )
        //{
        //    OpenFileDialog ofd = new OpenFileDialog ();
        //    ofd . InitialDirectory = Environment . GetFolderPath ( Environment . SpecialFolder . MyPictures );
        //    if ( ofd . ShowDialog ( this ) == true )
        //    {
        //        wallpaper . SetWallpaper ( null , ofd . FileName );
        //    }
        //}

        private async void buttonManualFetch_Click ( object sender , RoutedEventArgs e )
        {
            ResetFetchTimer ();
            await Fetcher . Fetch ();
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
            Default . FetchAmount = FetchAmount = int . Parse ( textBoxAmount . Text );

        }

        private void sourceNewest_Checked ( object sender , RoutedEventArgs e )
        {
            FetchSource = WordPressCategory . Index;
            Default . FetchSource = 0;
        }

        private void sourceYesterday_Checked ( object sender , RoutedEventArgs e )
        {
            FetchSource = WordPressCategory . Yesterday;
            Default . FetchSource = 1;
        }

        private void Window_Closing ( object sender , CancelEventArgs e )
        {
            e . Cancel = true;
            Default . Save ();
            Hide ();
            //NotifyIcon . Popup ( 3000 , "Icarus Wallpaper" , "I'm always here!" );
        }

        private void linkHomePage_Click ( object sender , RoutedEventArgs e )
        {
            Process . Start ( "http://icarus.silversky.moe:666/?from=Dwscdv3.IcarusWallpaper" );
        }

        private void wallpaperMainSwitch_Checked ( object sender , RoutedEventArgs e )
        {
            var wallpaperMainSwitch = (CheckBox) sender;
            Default . WallpaperMainSwitch = (bool) wallpaperMainSwitch . IsChecked;
            Default . WallpaperSetInterval = WallpaperSetInterval = TimeSpan . FromMinutes ( int . Parse ( textBoxInterval . Text ) );
        }

        private void wallpaperMainSwitch_Unchecked ( object sender , RoutedEventArgs e )
        {
            var wallpaperMainSwitch = (CheckBox) sender;
            Default . WallpaperMainSwitch = (bool) wallpaperMainSwitch . IsChecked;
            /*Default . WallpaperSetInterval = */WallpaperSetInterval = TimeSpan . Zero;
        }

        private void downloadPathButton_MouseRightButtonDown ( object sender , MouseButtonEventArgs e )
        {
            var downloadPathButton = (Button) sender;
            Default . DownloadPath = "";
            downloadPathButton . Content = "Set download path...";
            downloadPathButton . ToolTip = "Current working directory.\nRight click to reset.";
        }

        private void downloadPathButton_Click ( object sender , RoutedEventArgs e )
        {
            var fbd = new System . Windows . Forms . FolderBrowserDialog ();
            fbd . Description = "_(:зゝ∠)_";
            if ( fbd . ShowDialog () == System . Windows . Forms . DialogResult . OK )
            {
                var path = fbd . SelectedPath + "\\";
                setPath ( path );
            }
        }

        private void setPath ( string path )
        {
            downloadPathButton . Content = Default . DownloadPath = path;
            downloadPathButton . ToolTip = path + "\nRight click to reset.";
        }

        private void randomCheckBox_Checked ( object sender , RoutedEventArgs e )
        {
            Default . RandomWallpaper = true;
        }

        private void randomCheckBox_Unchecked ( object sender , RoutedEventArgs e )
        {
            Default . RandomWallpaper = false;
        }

        private void textBoxInterval_LostFocus ( object sender , RoutedEventArgs e )
        {
            if ( Default . WallpaperMainSwitch )
            {
                Default . WallpaperSetInterval = WallpaperSetInterval = TimeSpan . FromMinutes ( int . Parse ( textBoxInterval . Text ) );
            }
        }

        private void ratio_ValueChanged ( object sender , RoutedPropertyChangedEventArgs<double> e )
        {
            if ( ratioText != null )
            {
                ratioText . Text = e . NewValue . ToString ();
                Default . AspectRatioLimit = e . NewValue;
            }
        }

        private void filterCheckBox_Checked ( object sender , RoutedEventArgs e )
        {
            Default . FilterAspectRatio = (bool) filterCheckBox . IsChecked;
        }

        private void filterCheckBox_Unchecked ( object sender , RoutedEventArgs e )
        {
            Default . FilterAspectRatio = (bool) filterCheckBox . IsChecked;
        }
    }
}
