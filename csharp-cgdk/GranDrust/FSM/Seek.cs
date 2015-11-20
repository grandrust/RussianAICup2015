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
            ITargetState state = vehicle.NextTile() != vehicle.CurrentTile()
                ? (ITargetState)Arrive.Instance
                : FollowTo.Instance;

            state.TargetPoint = target;
            state.Execute(vehicle);
        }

        private Point Find(Vehicle vehicle)
        {
            return vehicle.Map.GetNextPoint(vehicle.Self.NextWaypointX, vehicle.Self.NextWaypointY);
        }
    }
}
