using System;
using GranDrust.GameEntities;
using GranDrust.Helpers;

// ReSharper disable once CheckNamespace
namespace GranDrust.FSM
{
    public class FollowTo : TargetState
    {
        private FollowTo()
        {
        }

        private static FollowTo _instantce;
        public static FollowTo Instance
        {
            get { return _instantce ?? (_instantce = new FollowTo()); }
        }

        public override void Execute(Vehicle vehicle)
        {
            var angleToWaypoint = vehicle.Self.GetAngleTo(TargetPoint.X, TargetPoint.Y);
            var speedModule = vehicle.Self.SpeedModule();


            vehicle.Move.WheelTurn = angleToWaypoint * 32.0D / Math.PI;
            vehicle.Move.EnginePower = 1.0D;

            

            if (speedModule*speedModule*Math.Abs(angleToWaypoint) > 10.0D*Math.PI)
            {
                vehicle.Move.IsBrake = true;
            }
        }
    }
}
