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

        private Point _nextPoint;
        private Point _obstaclePoint;

        public override void Enter(Vehicle vehicle)
        {
            var target = (ITargetState)vehicle.PreviousState;
            _nextPoint = target != null 
                            ? target.TargetPoint
                            : new Point();

            _obstaclePoint = vehicle.Self.CurrentPoint();
        }

        public override void Execute(Vehicle vehicle)
        {
            var angleTo = vehicle.Self.GetAngleTo(_nextPoint);
            var desiredAngle = Math.Sign(angleTo) > 0.0D 
                                    ? -Math.PI/2 
                                    : Math.PI/2;

            vehicle.Move.WheelTurn = desiredAngle * 32.0D / Math.PI;
            vehicle.Move.EnginePower = -1.0D;
        }
        
        public override void Update(Vehicle vehicle)
        {
            var angleTo = vehicle.Self.GetAngleTo(_nextPoint);

            //TODO: back wall possible

            var angleToNextPoint = Math.Abs(angleTo);
            var obstacleDistance = vehicle.Self.GetDistanceTo(_obstaclePoint);
            if (obstacleDistance > 1.5D* vehicle.Self.Width && angleToNextPoint < Math.PI / 5)
                vehicle.ChangeState(Stop.Instance);


            if (obstacleDistance > 0.5 * vehicle.Self.Width && vehicle.Self.SpeedModule() < 0.5D)
                vehicle.ChangeState(Stop.Instance);

        }
    }
}
