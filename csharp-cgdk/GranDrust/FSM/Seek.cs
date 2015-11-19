using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.GameEntities;
using GranDrust.Helpers;

// ReSharper disable once CheckNamespace
namespace GranDrust.FSM
{
    public class Seek: IState
    {
        public void Execute(Vehicle vehicle)
        {
            var target = Find(vehicle);
            var stratagy = new FollowTo { TargetPoint = target};

            stratagy.Execute(vehicle);
        }

        private Point Find(Vehicle vehicle)
        {
            var nextWaypointXIndex = vehicle.Self.NextWaypointX;
            var nextWaypointYIndex = vehicle.Self.NextWaypointX;

            double nextWaypointX = (nextWaypointXIndex + 0.5D) * vehicle.Game.TrackTileSize;
            double nextWaypointY = (nextWaypointYIndex + 0.5D) * vehicle.Game.TrackTileSize;
            var cornerTileOffset = 0.3D * vehicle.Game.TrackTileSize;

            switch (vehicle.World.TilesXY[nextWaypointXIndex][nextWaypointYIndex])
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

            return new Point(nextWaypointX, nextWaypointY);
        }
    }
}
