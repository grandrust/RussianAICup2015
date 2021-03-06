﻿using System;
using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.GameEntities;

namespace GranDrust.Helpers
{
    public static class ObstacleHelper
    {
        public static bool IsOutWay(this Vehicle vehicle, Point point, ref Point obstaclePoint)
        {
            var xIndex = GameHelper.GetTileIndex(vehicle.Self.X, vehicle.Game.TrackTileSize);
            var yIndex = GameHelper.GetTileIndex(vehicle.Self.Y, vehicle.Game.TrackTileSize);

            return InCorners(xIndex, yIndex, vehicle, point, ref obstaclePoint) || InLines(xIndex, yIndex, vehicle, ref obstaclePoint);
        }

        private static bool InLines(int xIndex, int yIndex, Vehicle vehicle, ref Point obstaclePoint)
        {
            var tileType = vehicle.World.TilesXY[xIndex][yIndex];

            var left = xIndex * vehicle.Game.TrackTileSize;
            var top = yIndex * vehicle.Game.TrackTileSize;
            var right = left + vehicle.Game.TrackTileSize;
            var bottom = top + vehicle.Game.TrackTileSize;

            var innerAngle = Math.Atan2(vehicle.Self.Height / 2, vehicle.Self.Width / 2);

            var offset = vehicle.Game.TrackTileMargin + 4.0D;

            switch (tileType)
            {
                case TileType.Horizontal:
                case TileType.LeftTopCorner:
                case TileType.RightTopCorner:
                case TileType.BottomHeadedT:
                    top = vehicle.Self.Y - top;
                    break;
            }

            if (top + vehicle.DiagonalLength() * DesiredAngle(vehicle, innerAngle, true) < offset)
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

            if (bottom - vehicle.DiagonalLength() * DesiredAngle(vehicle, innerAngle, true) < offset)
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

            if (left + vehicle.DiagonalLength() * DesiredAngle(vehicle, innerAngle, false) < offset)
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

            if (right - vehicle.DiagonalLength() * DesiredAngle(vehicle, innerAngle, false) < offset)
                return true;

            return false;
        }

        private static double DesiredAngle(Vehicle vehicle, double innerAngle, bool isVertical)
        {
            return isVertical
                ? Math.Sin(vehicle.Self.Angle + Math.Sign(Math.Tan(vehicle.Self.Angle)) * innerAngle)
                : Math.Cos(vehicle.Self.Angle - Math.Sign(Math.Tan(vehicle.Self.Angle)) * innerAngle);
        }

        private static bool InCorners(int xIndex, int yIndex, Vehicle vehicle, Point point, ref Point obstaclePoint)
        {
            var left = xIndex * vehicle.Game.TrackTileSize;
            var top = yIndex * vehicle.Game.TrackTileSize;
            var right = (xIndex + 1) * vehicle.Game.TrackTileSize;
            var bottom = (yIndex + 1) * vehicle.Game.TrackTileSize;

            var offset = vehicle.Game.TrackTileMargin + vehicle.DiagonalLength() + 1.5D;

            return MovementHelper.GetDistance(left, top, point.X, point.Y) < offset
                   || MovementHelper.GetDistance(right, top, point.X, point.Y) < offset
                   || MovementHelper.GetDistance(right, bottom, point.X, point.Y) < offset
                   || MovementHelper.GetDistance(left, bottom, point.X, point.Y) < offset;


        }
    }
}
