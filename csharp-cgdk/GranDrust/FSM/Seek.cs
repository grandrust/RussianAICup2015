using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.GameEntities;
using GranDrust.Helpers;

// ReSharper disable once CheckNamespace
namespace GranDrust.FSM
{
    public class Seek: IState
    {
        private Seek()
        {
        }

        private static Seek _instantce;
        public static Seek Instance
        {
            get { return _instantce ?? (_instantce = new Seek()); }
        }

        public void Execute(Vehicle vehicle)
        {
            var target = Find(vehicle);

            //temp impl
            if (vehicle.World.TilesXY[vehicle.Self.NextWaypointX][vehicle.Self.NextWaypointY] == TileType.LeftTopCorner)
            {
                var state = Arrive.Instance;
                state.TargetPoint = target;
                vehicle.State = state;
                state.Execute(vehicle);
                return;
            }

            var stratagy = FollowTo.Instance;
            stratagy.TargetPoint = target;

            stratagy.Execute(vehicle);
        }

        private Point Find(Vehicle vehicle)
        {
            var nextWaypointXIndex = vehicle.Self.NextWaypointX;
            var nextWaypointYIndex = vehicle.Self.NextWaypointY;

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

                case TileType.Vertical:
                    nextWaypointX = vehicle.Self.X;
                    nextWaypointY -= cornerTileOffset;
                    break;
            }

            return new Point(nextWaypointX, nextWaypointY);
        }
    }
}
