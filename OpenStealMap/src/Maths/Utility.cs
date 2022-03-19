
using System;
namespace OsmLib
{
    static class Utility
    {
        public static double ToRadians(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }
        public static double GetMetersPerPixel(int zoom, int y)
        {
            var earthCircumference = 40075016.686;
            var latitude = HemisphereDegreeCalculator.FromYToLatitude(y, zoom);
            var distancePerPixel = earthCircumference * Math.Cos(latitude) / Math.Pow(2, zoom + 8);
            return distancePerPixel;
        }
    }
}
