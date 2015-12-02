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
            if (HasObstacle(vehicle) && vehicle.Self.EnginePower > 0.85D && vehicle.Self.SpeedModule() < 0.1D)
            {
                vehicle.ChangeState(Reversal.Instance);
                return;
            }


            if (vehicle.Self.EnginePower > 0.93D && vehicle.Self.SpeedModule() < 0.1D && vehicle.World.Tick -3 > vehicle.Game.InitialFreezeDurationTicks )
            {
                vehicle.ChangeState(Reversal.Instance);
                return;
            }


            var distance = vehicle.Self.GetDistanceTo(TargetPoint);
            
            //TODO: fix choose path can stay in the same tile
            if (vehicle.InTheSameTile(TargetPoint)
                    || (distance <= MovementHelper.GetDistance(vehicle.Self.NextPoint(), TargetPoint)))
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