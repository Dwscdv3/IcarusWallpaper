using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using static IcarusWallpaper . Properties . Settings;

namespace IcarusWallpaper
{
    static class Parameters
    {
        public static string RegistryAppName
        {
            get
            {
                return "Icarus Wallpaper";
            }
        }
        public static string IcarusYesterdayUrl
        {
            get
            {
                return $"{Default . IcarusBase}/{ DateTime . Now . Year }/{ DateTime . Now . Month . ToString ( "D2" ) }/{ ( DateTime . Now . Day - 1 ) . ToString ( "D2" ) }";
            }
        }
        public static string IcarusNewestUrl
        {
            get
            {
                return Default . IcarusBase;
            }
        }
        //public static readonly Dictionary<string , string> IcarusCategoryUrl = new Dictionary<string , string>
        //{
        //    //{ "主页", IcarusBase },
        //    { "略低分壁纸", $"{IcarusBase}/category/illustration/little-low-res-wallpaper" },
        //    { "普分壁纸", $"{IcarusBase}/category/illustration/normal-res-wallpaper" },
        //    { "超高分壁纸", $"{IcarusBase}/category/illustration/super-res-wallpaper" },
        //    { "纵向壁纸", $"{IcarusBase}/category/illustration/phone-wallpaper" },
        //};

        //public static string DownloadPath { get; set; }
        public static int FetchAmount { get; set; }
        //public static WordPressCategory FetchSource { get; set; }
        //public static string FetchSource { get; set; } // Use settings instead
    }

    //enum WordPressCategory
    //{
    //    Index,
    //    Yesterday,
    //    Specified
    //}
}
