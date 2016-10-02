using System;
using System . Diagnostics;
using Microsoft . Win32;
using static IcarusWallpaper . Parameters;

namespace IcarusWallpaper
{
    static class AutoRun
    {
        static RegistryKey regRun = Registry . CurrentUser . OpenSubKey (
            @"Software\Microsoft\Windows\CurrentVersion\Run" , true );

        public static bool IsEnabled
        {
            get
            {
                return regRun . GetValue ( RegistryAppName ) != null;
            }
            set
            {
                if ( value )
                {
                    regRun . SetValue ( RegistryAppName , Process . GetCurrentProcess () . MainModule . FileName );
                }
                else
                {
                    regRun . DeleteValue ( RegistryAppName );
                }
            }
        }
    }
}
