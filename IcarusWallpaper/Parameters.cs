using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;

namespace IcarusWallpaper
{
    static class Parameters
    {
        public static string IcarusYesterdayRssUrl
        {
            get
            {
                return $"http://icarus.silversky.moe:666/{ DateTime . Now . Year }/{ DateTime . Now . Month . ToString ( "D2" ) }/{ ( DateTime . Now . Day - 1 ) . ToString ( "D2" ) }/rss";
            }
        }
        public static string IcarusNewestRssUrl
        {
            get
            {
                return "http://icarus.silversky.moe:666/rss";
            }
        }

        //public static string DownloadPath { get; set; }
        public static int FetchAmount { get; set; }
        public static WordPressCategory FetchSource { get; set; }
    }

    enum WordPressCategory
    {
        Index,
        Yesterday
    }
}
