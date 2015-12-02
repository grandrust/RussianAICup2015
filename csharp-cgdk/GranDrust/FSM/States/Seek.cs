using System;
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
            if (previousState != null)
            {
                if (_target == previousState.TargetPoint)
                {
                    _target = Find(vehicle); //vehicle.Map.GetNextPoint(vehicle.Self.NextWaypointIndex + 1);
                }
            }
        }

        public override void Execute(Vehicle vehicle)
        {
            _state = vehicle.NextTile() != vehicle.CurrentTile() //TODO: not true for T and crossroad use distance and angle
                ? (ITargetState)Arrive.Instance
                : FollowTo.Instance;

            //_state = Arrive.Instance;

            _state.TargetPoint = _target;
            _state.Execute(vehicle);
        }

        public override void Update(Vehicle vehicle)
        {
            vehicle.ChangeState(_state);
        }

        private Point Find(Vehicle vehicle)
        {
            var currentPoint = GetStartPoint(vehicle);
            var route = vehicle.CreateRouteOf(10, currentPoint);

            double nextWaypointX = (route[0].X + 0.5D) * vehicle.Game.TrackTileSize;
            double nextWaypointY = (route[0].Y + 0.5D) * vehicle.Game.TrackTileSize;

            var addOn = vehicle.Game.TrackTileSize * 0.4D;

            left = 0.0D;
            right = 0.0D;

            top = 0.0D;
            bottom = 0.0D;

            var startCell = vehicle.GetCurrentCell(currentPoint);

            if (IsOnLine(startCell, route[0], route[1], route[2]))
            {
                nextWaypointX = (route[0].X + 0.5D) * vehicle.Game.TrackTileSize;
                nextWaypointY = (route[0].Y + 0.5D) * vehicle.Game.TrackTileSize;
                NextWaypointApdate(route[1], route[0], 0.49 * vehicle.Game.TrackTileSize);

                return OptimalPoint(nextWaypointX, nextWaypointY);
            }


           // NextWaypointApdate(route[0], startCell, addOn);
            NextWaypointApdate(route[1], route[0], addOn);

            return OptimalPoint(nextWaypointX, nextWaypointY);
        }

        private bool IsOnLine(BFSearch.Cell startCell, BFSearch.Cell cell, BFSearch.Cell cell1, BFSearch.Cell cell2)
        {
            return startCell.X - cell.X == cell1.X - cell2.X && startCell.Y - cell.Y == cell1.Y - cell2.Y;
        }

        private bool IsOnLine(BFSearch.Cell startCell, BFSearch.Cell cell)
        {
            return Math.Abs(startCell.X - cell.X) - Math.Abs(startCell.Y - cell.Y) == 0;
        }

        private Point OptimalPoint(double nextWaypointX, double nextWaypointY)
        {
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

        private Point GetStartPoint(Vehicle vehicle)
        {
            return vehicle.PreviousState is Stop 
                ? vehicle.Self.CurrentPoint()
                : vehicle.NosePoint();
        }
    }
}
