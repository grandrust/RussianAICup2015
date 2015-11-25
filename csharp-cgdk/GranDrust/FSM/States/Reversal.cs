using System;
using GranDrust.GameEntities;
using GranDrust.Helpers;

namespace GranDrust.FSM.States
{
    public class Reversal : StateBase
    {
        private Reversal()
        {
        }

        private static Reversal _instantce;
        public static Reversal Instance
        {
            get { return _instantce ?? (_instantce = new Reversal()); }
        }

        private Point _initPoint;

        public override void Enter(Vehicle vehicle)
        {
            var target = (ITargetState)vehicle.PreviousState;
            _initPoint = target != null 
                            ? target.TargetPoint
                            : new Point();
        }

        public override void Execute(Vehicle vehicle)
        {
            var angleTo = vehicle.Self.GetAngleTo(_initPoint);
            var desiredAngle = Math.Sign(angleTo) > 0.0D 
                                    ? -Math.PI/2 
                                    : Math.PI/2;

            vehicle.Move.WheelTurn = desiredAngle * 32.0D / Math.PI;
            vehicle.Move.EnginePower = -1.0D;
        }
        
        public override void Update(Vehicle vehicle)
        {
            var angleTo = vehicle.Self.GetAngleTo(_initPoint);
            if (Math.Abs(angleTo) < Math.PI / 5)
                vehicle.ChangeState(Stop.Instance);
        }
    }
}
