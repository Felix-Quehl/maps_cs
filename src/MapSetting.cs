using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using OsmLib;

namespace OpenStealMap
{
    public class MapSetting
    {
        public string ApiKey { get; }
        public string Cache { get; }
        public string Name { get; }
        public List<string> Servers { get; }
        public int Zoom { get; }
        public double Top { get; }
        public double Bottom { get; }
        public double Left { get; }
        public double Right { get; }
        public bool Offline { get; }
        private int _nextServerIndex = 0;
        private Mutex _serverMonitor = new Mutex();

        public MapSetting(string[] args)
        {
            Servers = new List<string>();
            var parts = args.Select(p => p.Split('='));

            foreach (var part in parts)
            {
                var key = part[0].ToLower();
                string value;
                if (part.Length > 1)
                    value = part[1];
                else
                    value = "true";

                switch (key)
                {
                    case "--name":
                        Name = value;
                        break;
                    case "--server":
                        Servers.Add(value);
                        break;
                    case "--cache":
                        Cache = value;
                        break;
                    case "--apikey":
                        ApiKey = value;
                        break;
                    case "--offline":
                        Offline = bool.Parse(value);
                        break;
                    case "--zoom":
                        Zoom = int.Parse(value);
                        break;
                    case "--top":
                        Top = double.Parse(value);
                        break;
                    case "--bottom":
                        Bottom = double.Parse(value);
                        break;
                    case "--left":
                        Left = double.Parse(value);
                        break;
                    case "--right":
                        Right = double.Parse(value);
                        break;
                }
            }

            if (Zoom < 0 || Zoom > 20)
                throw new ArgumentException("invalid value for 'zoom' parameter");

            if (Top < -90 || Top > 90 || Top < Bottom)
                throw new ArgumentException("invalid value for 'top' parameter");

            if (Bottom < -90 || Bottom > 90 || Bottom > Top)
                throw new ArgumentException("invalid value for 'bottom' parameter");

            if (Left < -180 || Left > 180 || Left > Right)
                throw new ArgumentException("invalid value for 'left' parameter");

            if (Right < -180 || Right > 180 || Right < Left)
                throw new ArgumentException("invalid value for 'right' parameter");

            if (Name == default)
                throw new ArgumentException("missing 'name' parameter");

            if (Servers.Count == 0 && !Offline)
                throw new ArgumentException("missing 'server' or 'offline' parameter");

            if (Cache == default)
                throw new ArgumentException("missing 'cache' parameter");

        }

        public Rectangle GetArea()
        {
            var topLeftPoint = GetPoint(Top, Left);
            var buttomRightPoint = GetPoint(Bottom, Right);

            var width = buttomRightPoint.X - topLeftPoint.X;
            var height = buttomRightPoint.Y - topLeftPoint.Y;

            return new Rectangle(topLeftPoint.X, topLeftPoint.Y, width, height);
        }

        private Point GetPoint(double latitude, double longitude)
        {
            var y = TileIndexCaluclator.LatitudeToY(latitude, Zoom);
            var x = TileIndexCaluclator.LongiduteToX(longitude, Zoom);
            return new Point(x, y);
        }
        public string GetServer()
        {
            try
            {
                _serverMonitor.WaitOne(100);
                if (_nextServerIndex >= Servers.Count())
                    _nextServerIndex = 0;
                var result = Servers[_nextServerIndex];
                _nextServerIndex++;
                return result;
            }
            finally
            {
                _serverMonitor.ReleaseMutex();
            }
        }
    }
}
