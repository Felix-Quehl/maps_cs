using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OpenStealMap
{
    public class AreaDownloader : TileDownloader
    {
        public void DownloadArea(MapSetting mapSetting)
        {
            var area = mapSetting.GetArea();
            Console.WriteLine($"downloading {area.Height * area.Width} tiles...");

            var tileMissing = true;

            var cache = Directory.CreateDirectory(mapSetting.Cache);

            while (tileMissing)
            {
                var options = new ParallelOptions();
                options.MaxDegreeOfParallelism = Environment.ProcessorCount;
                tileMissing = false;
                Parallel.For(area.Left, area.Right + 1, options, x =>
                {
                    for (int y = area.Top; y <= area.Bottom; y++)
                    {
                        var fileName = $"{cache.FullName}{Path.DirectorySeparatorChar}{mapSetting.Zoom}_{x}_{y}.png";
                        var tileIsMissing = !File.Exists(fileName);
                        if (tileIsMissing)
                        {
                            var point = new Point(x, y);
                            var queryParameter = new Dictionary<string, string>();
                            if (mapSetting.ApiKey != default)
                                queryParameter.Add("ApiKey", mapSetting.ApiKey);
                            var server = mapSetting.GetServer();

                            while (tileIsMissing)
                                try
                                {
                                    var response = base.DownloadTile(server, mapSetting.Zoom, point, queryParameter);
                                    switch ((int)response.StatusCode)
                                    {
                                        case 200:
                                            var data = response.Content.ReadAsByteArrayAsync().Result;
                                            File.WriteAllBytes(fileName, data);
                                            Console.WriteLine($"fetched {mapSetting.Zoom}_{x}_{y} from {server}");
                                            tileIsMissing = false;
                                            break;
                                        case 429:
                                            Console.WriteLine($"{response.StatusCode} from {server}, retrying...");
                                            Thread.Sleep(10_000);
                                            break;
                                        case 503:
                                            Console.WriteLine($"{response.StatusCode} from {server}, retrying...");
                                            Thread.Sleep(1_000);
                                            break;
                                        default:
                                            throw new HttpRequestException($"unexpected code {response.StatusCode} from {response.RequestMessage.RequestUri}");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine($"Error: Download of {fileName} failed because {e.Message}!");
                                }
                        }
                    }
                });
            }
            Console.WriteLine($"download done");
        }
    }
}
