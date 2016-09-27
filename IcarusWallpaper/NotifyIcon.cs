using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Forms;

namespace IcarusWallpaper
{
    static class NotifyIcon
    {
        static System . Windows . Forms . NotifyIcon notifyIcon = null;
        static MainWindow window = null;

        public static void Create ( MainWindow window )
        {
            NotifyIcon . window = window;
            notifyIcon = new System . Windows . Forms . NotifyIcon ();
            notifyIcon . Text = "把你伊卡鲁斯掉（";
            notifyIcon . Icon = System . Drawing . Icon . ExtractAssociatedIcon ( Application . ExecutablePath );
            notifyIcon . MouseClick += ( sender , e ) =>
            {
                if ( e . Button == MouseButtons . Left )
                {
                    window . Show ();
                    window . Activate ();
                }
            };
            notifyIcon . ContextMenu = new ContextMenu ( new MenuItem []
            {
                new MenuItem ( "Exit" , ( sender , e ) =>
                {
                    notifyIcon . Visible = false;
                    Properties . Settings . Default . Save ();
                    Environment . Exit ( 0 );
                } )
            } );
            notifyIcon . Visible = true;
        }

        public static void Popup ( int timeout , string title , string text )
        {
            notifyIcon . ShowBalloonTip ( timeout , title , text , ToolTipIcon . None );
        }
    }
}
