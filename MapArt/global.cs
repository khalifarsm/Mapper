using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Controls;
using DotSpatial.Projections;

namespace MapArt
{
    public static class global
    {
        public
        static string layername = "ligne";
        public static MapLineLayer linelyr = new MapLineLayer();
        public static MapRasterLayer demlyr=null;
        public static ProjectionInfo pend = new ProjectionInfo();
        public static Map map;
        public static bool digitizingbool=false;
        public static bool mesurebool = false;
        public static bool mesurebool2 = false;
    }
}
