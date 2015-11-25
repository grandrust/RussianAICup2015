using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.GameEntities;
using GranDrust.Helpers;
using GranDrust.Search;

namespace GranDrust.FSM.States
{
    public class Seek : StateBase
    {
        private ITargetState _state;
        private Point _target;

        private Seek()
        {
        }

        private static Seek _instantce;
        public static Seek Instance
        {
            get { return _instantce ?? (_instantce = new Seek()); }
        }

        public override void Enter(Vehicle vehicle)
        {

            var route = new BFSearch(vehicle).Search();

            //_target = Find(vehicle);
            _target = Find(vehicle, route[0]);

            var previousState = vehicle.PreviousState as ITargetState;
            if (previousState != null)
            {
                if (_target == previousState.TargetPoint)
                {
                    _target = Find(vehicle, route[1]); //vehicle.Map.GetNextPoint(vehicle.Self.NextWaypointIndex + 1);
                }
            }
        }

        public override void Execute(Vehicle vehicle)
        {
            _state = vehicle.NextTile() != vehicle.CurrentTile() //TODO: not true for T and crossroad use distance and angle
                ? (ITargetState)Arrive.Instance
                : FollowTo.Instance;

            _state.TargetPoint = _target;
            _state.Execute(vehicle);
        }

        public override void Update(Vehicle vehicle)
        {
            vehicle.ChangeState(_state);
        }

        private Point Find(Vehicle vehicle)
        {
            return vehicle.Map.GetNextPoint(vehicle.Self.NextWaypointX, vehicle.Self.NextWaypointY);
        }
        private Point Find(Vehicle vehicle, BFSearch.Cell cell)
        {
            double nextWaypointX = (cell.X + 0.5D) * vehicle.Game.TrackTileSize;
            double nextWaypointY = (cell.Y + 0.5D) * vehicle.Game.TrackTileSize;

            var cornerTileOffset = 0.3D * vehicle.Game.TrackTileSize;

            switch (vehicle.World.TilesXY[cell.X][cell.Y])
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
