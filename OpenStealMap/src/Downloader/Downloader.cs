using System;
using System.Net;
using System.Net.Http;

namespace OpenStealMap
{
    public class Downloader : HttpClient
    {
        public HttpResponseMessage Download(HttpRequestMessage request)
        {
            return base.SendAsync(request).Result;
        }
    }
}
