using System;
namespace OsmLib
{
    public static class HemisphereDegreeCalculator
    {
        public static double FromXToLongitude(int longitude, int zoom)
        {
            return longitude / (double)(1 << zoom) * 360.0 - 180;
        }
        public static double FromYToLatitude(int latitue, int zoom)
        {
            double n = Math.PI - 2.0 * Math.PI * latitue / (double)(1 << zoom);
            return 180.0 / Math.PI * Math.Atan(0.5 * (Math.Exp(n) - Math.Exp(-n)));
        }
    }
}
