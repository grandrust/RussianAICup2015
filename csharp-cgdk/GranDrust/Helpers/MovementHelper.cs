using System;
using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;

// ReSharper disable once CheckNamespace
namespace GranDrust.Helpers
{
    public static class MovementHelper
    {
        public static double SpeedModule(this Car car)
        {
            return Math.Sqrt(car.SpeedX*car.SpeedX + car.SpeedY*car.SpeedY);
        }

        public static double GetDistanceTo(this Car car, Point target)
        {
            return car.GetDistanceTo(target.X, target.Y);
        }

        public static double GetAngleTo(this Car car, Point target)
        {
            return car.GetAngleTo(target.X, target.Y);
        }

        public static Point NextPoint(this Car car)
        {
            return new Point(car.X + car.SpeedX, car.Y + car.SpeedY);
        }
    }
}
