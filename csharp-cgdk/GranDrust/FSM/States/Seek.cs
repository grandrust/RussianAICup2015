using System.Collections.Generic;
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
        private double left;
        private double right;
        private double top;
        private double bottom;

        public static Seek Instance
        {
            get { return _instantce ?? (_instantce = new Seek()); }
        }

        public override void Enter(Vehicle vehicle)
        {
            _target = Find(vehicle); //TODO: nose point

            var previousState = vehicle.PreviousState as ITargetState;
            //if (previousState != null)
            //{
            //    if (_target == previousState.TargetPoint)
            //    {
            //        _target = Find(vehicle, route[1]); //vehicle.Map.GetNextPoint(vehicle.Self.NextWaypointIndex + 1);
            //    }
            //}
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
            var currentPoint = vehicle.Self.CurrentPoint();
            var current = new BFSearch.Cell(
                GameHelper.GetTileIndex(currentPoint.X, vehicle.Game.TrackTileSize),
                GameHelper.GetTileIndex(currentPoint.Y, vehicle.Game.TrackTileSize)
                );
            var terminate = vehicle.Map.GetNextPoint(vehicle.Self.NextWaypointX, vehicle.Self.NextWaypointY);
            var route = GetRoute(vehicle, current, new BFSearch.Cell(terminate.X, terminate.Y));

            var currentCell = route[0];
            var nextTerminate = vehicle.Map.GetNextPoint(vehicle.Self.NextWaypointIndex + 1);
            var nextCell = route.Count > 1 ?  route[1] : GetRoute(vehicle, new BFSearch.Cell(terminate.X, terminate.Y), new BFSearch.Cell(nextTerminate.X, nextTerminate.Y))[0]; 

            double nextWaypointX = (currentCell.X + 0.5D) * vehicle.Game.TrackTileSize;
            double nextWaypointY = (currentCell.Y + 0.5D) * vehicle.Game.TrackTileSize;

            var addOn = vehicle.Game.TrackTileSize * 0.4D;

            left = 0.0D;
            right = 0.0D;

            top = 0.0D;
            bottom = 0.0D;

            NextWaypointApdate(currentCell, current, addOn);
            NextWaypointApdate(nextCell, currentCell, addOn*1.1);

            nextWaypointX += left + right;
            nextWaypointY += top + bottom;

            return new Point(nextWaypointX, nextWaypointY);
        }

        private void NextWaypointApdate(BFSearch.Cell nextCell, BFSearch.Cell currentCell, double addOn)
        {

            if (nextCell.X > currentCell.X)
                right = addOn;

            if (nextCell.X < currentCell.X)
                left = -addOn;

            if (nextCell.Y < currentCell.Y)
                top = -addOn;

            if (nextCell.Y > currentCell.Y)
                bottom = addOn;
        }

        private static IList<BFSearch.Cell> GetRoute(Vehicle vehicle, BFSearch.Cell current, BFSearch.Cell terminate)
        {
            return new BFSearch(vehicle, current, terminate).Search();
        }
    }
}
