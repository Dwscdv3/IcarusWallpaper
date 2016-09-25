using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Diagnostics;
using System . IO;
using System . Net;
using System . Net . Http;
using System . Runtime . InteropServices;
using System . Text;
using System . Text . RegularExpressions;
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
using IcarusWallpaper . Native;
using Microsoft . Win32;
//using static IcarusWallpaper . Fetcher;
using static IcarusWallpaper . Parameters;
using static IcarusWallpaper . Timer;
using static IcarusWallpaper . Properties . Settings;

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
            Fetcher . BindWindow ( this );
            DownloadPath = @"G:\Icarus\";
            textBoxAmount . Text = Default . FetchAmount . ToString ();
            FetchAmount = Default . FetchAmount;
            #region FetchSource
            switch ( Default.FetchSource )
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
            //FetchInterval = TimeSpan . FromSeconds ( 3 );
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

        private async void buttonManualFetch_Click ( object sender , RoutedEventArgs e )
        {
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
            FetchAmount = int . Parse ( textBoxAmount . Text );
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
            Default . Save ();
        }

        private void linkHomePage_Click ( object sender , RoutedEventArgs e )
        {
            Process . Start ( "http://icarus.silversky.moe:666/?from=Dwscdv3.IcarusWallpaper" );
        }
    }
}
