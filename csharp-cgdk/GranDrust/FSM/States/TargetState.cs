using GranDrust.GameEntities;
using GranDrust.Helpers;

namespace GranDrust.FSM.States
{
    public class TargetState : StateBase, ITargetState
    {
        public Point TargetPoint { get; set; }

        public override void Update(Vehicle vehicle)
        { 
            if (HasObstacle(vehicle))
            {
                vehicle.ChangeState(Reversal.Instance);
            }

            var distance = vehicle.Self.GetDistanceTo(TargetPoint);
            
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

            return vehicle.IsOutWay(nextPoint) && vehicle.Self.SpeedModule() < 0.1D;

        }
    }
}