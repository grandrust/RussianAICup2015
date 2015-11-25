using GranDrust.GameEntities;
using GranDrust.Helpers;

namespace GranDrust.FSM.States
{
    public class Stop : StateBase
    {
        private Stop()
        {
        }

        private static Stop _instantce;
        public static Stop Instance
        {
            get { return _instantce ?? (_instantce = new Stop()); }
        }

        public override void Execute(Vehicle vehicle)
        {
            vehicle.Move.WheelTurn = 0.0D;
            vehicle.Move.IsBrake = true;
        }

        public override void Update(Vehicle vehicle)
        {
            if (vehicle.Self.SpeedModule() < 0.2D)
                vehicle.ChangeState(Seek.Instance);
        }
    }
}
