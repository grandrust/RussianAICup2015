using System;
using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.GameEntities;

// ReSharper disable once CheckNamespace
namespace GranDrust.Helpers
{
    public static class GameHelper
    {
        public static TileType NextTile(this Vehicle vehicle)
        {
            return vehicle.World.TilesXY[vehicle.Self.NextWaypointX][vehicle.Self.NextWaypointY];
        }

        public static TileType NextNTile(this Vehicle vehicle, int n)
        {
            var index = (vehicle.Self.NextWaypointIndex + n) % vehicle.World.Waypoints.Length;
            var point = vehicle.World.Waypoints[index];

            return vehicle.World.TilesXY[point[0]][point[1]];
        }

        public static TileType CurrentTile(this Vehicle vehicle)
        {
            var x = GetTileIndex(vehicle.Self.X, vehicle.Game.TrackTileSize);
            var y = GetTileIndex(vehicle.Self.Y,  vehicle.Game.TrackTileSize);

            return vehicle.World.TilesXY[x][y];
        }

        public static bool InTheSameTile(this Vehicle vehicle, Point point)
        {
            var currentPoint = vehicle.NosePoint();
            var x = GetTileIndex(currentPoint.X, vehicle.Game.TrackTileSize);
            var y = GetTileIndex(currentPoint.Y, vehicle.Game.TrackTileSize);
            var pointX = GetTileIndex(point.X, vehicle.Game.TrackTileSize);
            var pointY = GetTileIndex(point.Y, vehicle.Game.TrackTileSize);

            return x - pointX == 0 && y - pointY == 0;
        }

        public static int GetTileIndex(double coordinate, double tileSize)
        {
            return Convert.ToInt32(Math.Floor(coordinate / tileSize));
        }

        public static Point NosePoint(this Vehicle vehicle)
        {
            return new Point(vehicle.Self.X + Math.Cos(vehicle.Self.Angle)*vehicle.DiagonalLength(),
                vehicle.Self.Y + Math.Sin(vehicle.Self.Angle)*vehicle.DiagonalLength());
        }

        public static double DiagonalLength(this Vehicle vehicle)
        {
            var x = vehicle.Self.Width / 2;
            var y = vehicle.Self.Height / 2;

            return Math.Sqrt(x * x + y * y);
        }
    }
}
