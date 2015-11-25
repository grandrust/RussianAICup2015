using GranDrust.GameEntities;
using GranDrust.Helpers;

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
            _target = Find(vehicle);

            var previousState = vehicle.PreviousState as ITargetState;
            if (previousState != null)
            {
                if (_target == previousState.TargetPoint)
                {
                    _target = vehicle.Map.GetNextPoint(vehicle.Self.NextWaypointIndex + 1);
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
    }
}
