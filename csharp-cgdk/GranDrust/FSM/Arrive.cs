using System;
using GranDrust.GameEntities;
using GranDrust.Helpers;

// ReSharper disable once CheckNamespace
namespace GranDrust.FSM
{
    public class Arrive: ITargetState
    {
        private const double Deceleration = 0.3D;
        public Point TargetPoint { get; set; }

        private Arrive()
        {
        }

        private static Arrive _instantce;
        public static Arrive Instance
        {
            get { return _instantce ?? (_instantce = new Arrive()); }
        }
        public void Execute(Vehicle vehicle)
        {
            var distance = vehicle.Self.GetDistanceTo(TargetPoint);

            if (distance < vehicle.Self.Height * 0.5D) //TODO: fix it
            {
                vehicle.State = Seek.Instance;
                vehicle.State.Execute(vehicle);
                return;
            }

            var speedModule = vehicle.Self.SpeedModule();

            var speed = distance*Deceleration;
            speed = Math.Min(speed, speedModule);
            var nextPoint = vehicle.Self.NextPoint();

            var angleToWaypoint = AngleTo(TargetPoint.X, TargetPoint.Y, nextPoint.X, nextPoint.Y, vehicle.Self.Angle);

            vehicle.Move.WheelTurn = angleToWaypoint * 32.0D / Math.PI;
            vehicle.Move.EnginePower = speed / speedModule;

            if (speedModule * speedModule * Math.Abs(angleToWaypoint) > 9.5D * Math.PI)
            {
                vehicle.Move.IsBrake = true;
            }
        }

        private double AngleTo(double x, double y, double carX, double carY, double angle)
        {
            double absoluteAngleTo = Math.Atan2(y - carY, x - carX);
            double relativeAngleTo = absoluteAngleTo - angle;

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
