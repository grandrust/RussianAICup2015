using System;
using GranDrust.GameEntities;
using GranDrust.Helpers;

// ReSharper disable once CheckNamespace
namespace GranDrust.FSM
{
    public class Arrive: TargetState
    {
        private const double Deceleration = 0.03D;

        private Arrive()
        {
        }

        private static Arrive _instantce;
        public static Arrive Instance
        {
            get { return _instantce ?? (_instantce = new Arrive()); }
        }

        public override void Execute(Vehicle vehicle)
        {
            var distance = vehicle.Self.GetDistanceTo(TargetPoint);

            var speedModule = vehicle.Self.SpeedModule();

            var speed = distance / (Deceleration * (vehicle.Self.AngularSpeed + vehicle.Game.TrackTileSize));

            var angleToWaypoint = AngleToNextPoint(TargetPoint, vehicle);
            
            vehicle.Move.WheelTurn = angleToWaypoint * 32.0D / Math.PI;
            vehicle.Move.EnginePower = speed / Math.Cos(angleToWaypoint) * speedModule;


            vehicle.Move.EnginePower = Math.Abs(speedModule) < 0.1 ? 1.0D : speed / speedModule;

            //if (speedModule * speedModule * Math.Abs(angleToWaypoint) > 12.0D * Math.PI)
            //{
            //    vehicle.Move.IsBrake = true;
            //}


            if (Math.Cos(angleToWaypoint) * speedModule > speed)
            {
                vehicle.Move.IsBrake = true;
            }
        }

        private double AngleToNextPoint(Point target, Vehicle vehicle) //TODO: Find another way
        {
            var nextPoint = vehicle.Self.NextPoint();
            double absoluteAngleTo = Math.Atan2(target.Y - nextPoint.Y, target.X - nextPoint.X);
            double relativeAngleTo = absoluteAngleTo - vehicle.Self.Angle;

            while (relativeAngleTo > Math.PI)
            {
                relativeAngleTo -= 2.0D * Math.PI;
            }

            while (relativeAngleTo < -Math.PI)
            {
                relativeAngleTo += 2.0D * Math.PI;
            }

            return relativeAngleTo;
        }
    }
}
