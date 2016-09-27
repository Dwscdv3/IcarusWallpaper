using System;
using System . Collections . Generic;
using System . Configuration;
using System . Data;
using System . Linq;
using System . Threading . Tasks;
using System . Windows;
using static IcarusWallpaper . Util;
using static IcarusWallpaper . Native . HandleWindow;
using static IcarusWallpaper . Properties . Settings;
using System . IO;

namespace IcarusWallpaper
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup ( object sender , StartupEventArgs e )
        {
            //var instance = GetRunningInstance ();
            //if ( instance != null )
            if ( IsAnotherInstanceExist () )
            {
                //MessageBox . Show ( "Icarus Wallpaper is already running." , "Icarus Wallpaper" );

                //var hWnd = new IntPtr ( Default . WindowHandle );
                //ShowWindowAsync ( hWnd , 5 );
                //SetForegroundWindow ( hWnd );

                File . Create ( AppDomain . CurrentDomain . BaseDirectory + "Icarus_Show" );

                Environment . Exit ( -1 );
            }
        }
    }
}
