using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OsmLib;

namespace OpenStealMap
{
    class MapTileMerger
    {
        private const int TileSize = 256;
        public void Merge(MapSetting mapSetting)
        {
            var cache = Directory.CreateDirectory(mapSetting.Cache);
            var area = mapSetting.GetArea();
            var zoom = mapSetting.Zoom;

            var targetWidth = area.Width * TileSize + TileSize;
            var targetHeight = area.Height * TileSize + TileSize;
            var size = (uint)(targetWidth * targetHeight * 24);

            Console.WriteLine($"merging {area.Height * area.Width} tiles into {size} bytes...");

            using (var graphicFile = new Bitmap(targetWidth, targetHeight))
            using (var graphic = Graphics.FromImage(graphicFile))
            {
                for (int yIndex = area.Top; yIndex <= area.Bottom; yIndex++)
                    for (int xIndex = area.Left; xIndex <= area.Right; xIndex++)
                    {
                        var fileName = $"{cache.FullName}{Path.DirectorySeparatorChar}{zoom}_{xIndex}_{yIndex}.png";
                        if (File.Exists(fileName))
                            using (var tileStream = Image.FromFile(fileName))
                            {
                                int yPosition = (yIndex - area.Top) * TileSize;
                                int xPosition = (xIndex - area.Left) * TileSize;
                                graphic.DrawImage(tileStream, xPosition, yPosition);
                            }
                        else
                            Console.WriteLine($"Warning about missing tile {fileName}!");
                    }
                DrawGrid(graphic, mapSetting);
                graphic.Save();
                graphicFile.Save(mapSetting.Name, ImageFormat.Png);
            }
            Console.WriteLine($"merge done");
        }

        private void DrawGrid(Graphics graphic, MapSetting mapSetting)
        {
            var area = mapSetting.GetArea();
            var zoom = mapSetting.Zoom;

            var font = new Font(FontFamily.GenericMonospace, 14, FontStyle.Bold, GraphicsUnit.Pixel);
            DrawLongitudeLines(graphic, area, zoom, font);
            DrawLatitudeLines(graphic, area, zoom, font);
        }

        private static void DrawLatitudeLines(Graphics graphic, Rectangle area, int zoom, Font font)
        {
            for (int y = area.Top; y <= area.Bottom; y++)
            {
                int position = (y - area.Top) * TileSize;
                graphic.DrawLine(Pens.Black, 0, position, (1 + area.Width) * TileSize, position);
                var lat = HemisphereDegreeCalculator.FromYToLatitude(y, zoom);
                graphic.DrawString($"LAT \t{lat}", font, Brushes.Black, 0, position);
                graphic.DrawString($"LAT \t{lat}", font, Brushes.Black, (area.Width) * TileSize, position);
            }
        }

        private void DrawLongitudeLines(Graphics graphic, Rectangle area, int zoom, Font font)
        {
            for (int x = area.Left; x <= area.Right; x++)
            {
                int position = (x - area.Left) * TileSize;
                graphic.DrawLine(Pens.Black, position, 0, position, (1 + area.Height) * TileSize);
                var lon = HemisphereDegreeCalculator.FromXToLongitude(x, zoom);
                graphic.DrawString($"LONG \t{lon}", font, Brushes.Black, position, 15);
                graphic.DrawString($"LONG \t{lon}", font, Brushes.Black, position, (area.Height) * TileSize + 15);
            }
        }
    }
}

