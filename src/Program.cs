using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenStealMap
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var mapSettings = new MapSetting(args);

            Console.WriteLine($"*************************************");
            Console.WriteLine($"Making map {mapSettings.Name}");

            if (!mapSettings.Offline)
            {
                var areaDownloader = new AreaDownloader();
                areaDownloader.DownloadArea(mapSettings);
            }
            else
                Console.WriteLine("Offline-Mode, Skipping Downloading!");

            var MapMerger = new MapTileMerger();
            MapMerger.Merge(mapSettings);

            Console.WriteLine("Done");
        }

    }
}
