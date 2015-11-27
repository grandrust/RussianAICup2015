using GranDrust.GameEntities;
using GranDrust.Helpers;

namespace GranDrust.FSM.States
{
    public class TargetState : StateBase, ITargetState
    {
        public Point TargetPoint { get; set; }
        private Point _obstaclePoint;

        public override void Update(Vehicle vehicle)
        { 
            if (HasObstacle(vehicle))
            {
                vehicle.ChangeState(Reversal.Instance);
                return;
            }

            var distance = vehicle.Self.GetDistanceTo(TargetPoint);
            
            //TODO: fix choose path can stay in the same tile
            if (vehicle.InTheSameTile(TargetPoint)
                    || (distance <= MovementHelper.GetDistance(vehicle.Self.NextPoint(), TargetPoint) 
                            && vehicle.Map.GetNextPoint(vehicle.Self.NextWaypointIndex) != TargetPoint))
            {
                vehicle.ChangeState(Seek.Instance);
            }
        }

        private bool HasObstacle(Vehicle vehicle)
        {
            var nextPoint = vehicle.Self.NextPoint();

            return vehicle.Self.SpeedModule() < 0.1D && vehicle.IsOutWay(nextPoint, ref _obstaclePoint);

        }
    }
}