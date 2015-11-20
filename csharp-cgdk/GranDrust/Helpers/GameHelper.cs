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
        public static TileType CurrentTile(this Vehicle vehicle)
        {
            var X = Convert.ToInt32(Math.Floor(vehicle.Self.X / vehicle.Game.TrackTileSize));
            var Y = Convert.ToInt32(Math.Floor(vehicle.Self.Y / vehicle.Game.TrackTileSize));

            return vehicle.World.TilesXY[X][Y];
        }
    }
}
