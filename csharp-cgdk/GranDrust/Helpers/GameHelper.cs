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
            var x = GetTileIndex(vehicle.Self.X, vehicle.Game.TrackTileSize);
            var y = GetTileIndex(vehicle.Self.Y, vehicle.Game.TrackTileSize);
            var pointX = GetTileIndex(point.X, vehicle.Game.TrackTileSize);
            var pointY = GetTileIndex(point.Y, vehicle.Game.TrackTileSize);

            return x - pointX == 0 && y - pointY == 0;
        }

        public static int GetTileIndex(double coordinate, double tileSize)
        {
            return Convert.ToInt32(Math.Floor(coordinate / tileSize));
        }

        public static bool IsOutWay(this Vehicle vehicle, Point point)
        {
            var xIndex = GetTileIndex(vehicle.Self.X, vehicle.Game.TrackTileSize);
            var yIndex = GetTileIndex(vehicle.Self.Y, vehicle.Game.TrackTileSize);

            return InCorners(xIndex, yIndex, vehicle, point) || InLines(xIndex, yIndex, vehicle, point);
        }

        private static bool InLines(int xIndex, int yIndex, Vehicle vehicle, Point point)
        {
            var tileType = vehicle.World.TilesXY[xIndex][yIndex];

            var left = xIndex * vehicle.Game.TrackTileSize;
            var top = yIndex * vehicle.Game.TrackTileSize;
            var right = left + vehicle.Game.TrackTileSize;
            var bottom = top + vehicle.Game.TrackTileSize;

            var angle = GetMaxDistanceAngle(vehicle);

            var offset = vehicle.Game.TrackTileMargin * 1.01D; // TODO: FIX IT

            switch (tileType)
            {
                case TileType.Horizontal:
                case TileType.LeftTopCorner:
                case TileType.RightTopCorner:
                case TileType.BottomHeadedT:
                    top = vehicle.Self.Y - top;
                    break;
            }

            if (top + DiagonalLength(vehicle) * Math.Sin(angle) < offset)
                return true;

            switch (tileType)
            {
                case TileType.Horizontal:
                case TileType.LeftBottomCorner:
                case TileType.RightBottomCorner:
                case TileType.TopHeadedT:
                    bottom = bottom - vehicle.Self.Y;
                    break;
            }

            if (bottom - DiagonalLength(vehicle) * Math.Sin(angle) < offset)
                return true;

            switch (tileType)
            {
                case TileType.Vertical:
                case TileType.LeftBottomCorner:
                case TileType.LeftTopCorner:
                case TileType.RightHeadedT:
                    left = vehicle.Self.X - left;
                    break;
            }

            if (left + DiagonalLength(vehicle) * Math.Cos(angle) < offset)
                return true;

            switch (tileType)
            {
                case TileType.Vertical:
                case TileType.RightBottomCorner:
                case TileType.RightTopCorner:
                case TileType.LeftHeadedT:
                    right = right - vehicle.Self.X;
                    break;
            }

            if (right - DiagonalLength(vehicle) * Math.Cos(angle) < offset)
                return true;

            return false;
        }

        private static double GetMaxDistanceAngle(Vehicle vehicle)
        {
            var width = vehicle.Self.Width/2;
            var height = vehicle.Self.Height/2;
            var innerAngle = Math.Atan2(height, width);

            var angle = vehicle.Self.Angle - Math.Sign(vehicle.Self.Angle)*innerAngle;
            return angle;
        }

        private static bool InCorners(int xIndex, int yIndex, Vehicle vehicle, Point point)
        {
            var left = xIndex * vehicle.Game.TrackTileSize;
            var top = yIndex * vehicle.Game.TrackTileSize;
            var right = (xIndex + 1) * vehicle.Game.TrackTileSize;
            var bottom = (yIndex + 1) * vehicle.Game.TrackTileSize;

            var angle = GetMaxDistanceAngle(vehicle);

            var offset = vehicle.Game.TrackTileMargin + DiagonalLength(vehicle);

            return MovementHelper.GetDistance(left, top, point.X, point.Y) < offset
                   || MovementHelper.GetDistance(right, top, point.X, point.Y) < offset
                   || MovementHelper.GetDistance(right, bottom, point.X, point.Y) < offset
                   || MovementHelper.GetDistance(left, bottom, point.X, point.Y) < offset;


        }

        private static double DiagonalLength(Vehicle vehicle)
        {
            var x = vehicle.Self.Width/2;
            var y = vehicle.Self.Height/2;

            return Math.Sqrt(x*x + y*y);
        }
    }
}
