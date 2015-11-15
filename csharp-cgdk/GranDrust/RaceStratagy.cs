using System;
using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk;
using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;

// ReSharper disable once CheckNamespace
namespace GranDrust
{
    public class RaceStratagy : IStrategy
    {
        public void Move(Car self, World world, Game game, Move move)
        {
            double nextWaypointX = (self.NextWaypointX + 0.5D) * game.TrackTileSize;
            double nextWaypointY = (self.NextWaypointY + 0.5D) * game.TrackTileSize;
            var cornerTileOffset = 0.3D * game.TrackTileSize;

            switch (world.TilesXY[self.NextWaypointX][self.NextWaypointY])
            {
                case TileType.LeftTopCorner:
                    nextWaypointX += cornerTileOffset;
                    nextWaypointY += cornerTileOffset;
                    break;
                case TileType.RightTopCorner:
                    nextWaypointX -= cornerTileOffset;
                    nextWaypointY += cornerTileOffset;
                    break;
                case TileType.RightBottomCorner:
                    nextWaypointX -= cornerTileOffset;
                    nextWaypointY -= cornerTileOffset;
                    break;
                case TileType.LeftBottomCorner:
                    nextWaypointX += cornerTileOffset;
                    nextWaypointY -= cornerTileOffset;
                    break;
            }

            double angleToWaypoint = self.GetAngleTo(nextWaypointX, nextWaypointY);
            double speedModule = Math.Sqrt(self.SpeedX * self.SpeedX + self.SpeedY * self.SpeedY);

            move.WheelTurn = angleToWaypoint * 32.0D / Math.PI;
            move.EnginePower = 1.0D;

            if (world.Tick == 180)
                move.WheelTurn = 0.0d;

            if (speedModule * speedModule * Math.Abs(angleToWaypoint) > 3.0D * 2.5D * Math.PI)
            {
                move.IsBrake = true;
            }

            if ((self.X > nextWaypointX - 10) && (self.X < nextWaypointX + 10))
            {
                move.IsSpillOil = true;
            }
        }
    }
}
