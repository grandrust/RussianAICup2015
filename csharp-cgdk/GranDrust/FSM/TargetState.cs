using GranDrust.GameEntities;
using GranDrust.Helpers;

// ReSharper disable once CheckNamespace
namespace GranDrust.FSM
{
    public class TargetState : StateBase, ITargetState
    {
        public Point TargetPoint { get; set; }

        public override void Update(Vehicle vehicle)
        {
            var distance = vehicle.Self.GetDistanceTo(TargetPoint);
            
            if (vehicle.InTheSameTile(TargetPoint)
                    || (distance <= MovementHelper.GetDistance(vehicle.Self.NextPoint(), TargetPoint) 
                            && vehicle.Map.GetNextPoint(vehicle.Self.NextWaypointIndex) != TargetPoint))
            {
                vehicle.ChangeState(Seek.Instance);
            }
        }
    }
}