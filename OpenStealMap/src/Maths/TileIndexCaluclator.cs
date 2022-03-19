using System;
namespace OsmLib
{
    public static class TileIndexCaluclator
    {
        public static int LongiduteToX(double longitude, int zoom)
        {
            return (int)(Math.Floor(LongiduteToXDecimals(longitude, zoom)));
        }

        public static double LongiduteToXDecimals(double longitude, int zoom)
        {
            return (longitude + 180.0) / 360.0 * (1 << zoom);
        }

        public static int LatitudeToY(double latitude, int zoom)
        {
            return (int)Math.Floor(LatitudeToYDecimals(latitude, zoom));
        }

        public static double LatitudeToYDecimals(double latitude, int zoom)
        {
            return (1 - Math.Log(Math.Tan(Utility.ToRadians(latitude)) + 1 / Math.Cos(Utility.ToRadians(latitude))) / Math.PI) / 2 * (1 << zoom);
        }

    }
}
