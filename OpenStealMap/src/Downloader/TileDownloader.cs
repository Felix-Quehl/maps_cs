using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;

namespace OpenStealMap
{
    public class TileDownloader : Downloader
    {
        public HttpResponseMessage DownloadTile(string server, int zoom, Point point, Dictionary<string, string> queryParameter)
        {
            var url = $"{server}/{zoom}/{point.X}/{point.Y}.png";
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                if (queryParameter.Any())
                {
                    url += '?';
                    foreach (var parameter in queryParameter)
                        url += $"{parameter.Key}={parameter.Value}";
                }
                request.Headers.Add("User-Agent", "tiles2png");
                return base.Download(request);
            }
        }
    }
}
