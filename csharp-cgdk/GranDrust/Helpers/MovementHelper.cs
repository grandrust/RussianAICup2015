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

        public static double GetDistance(Point from, Point to)
        {
            double xRange = from.X - to.X;
            double yRange = from.Y - to.Y;
            return Math.Sqrt(xRange * xRange + yRange * yRange);
        }

        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            double xRange = x1 - x2;
            double yRange = y1 - y2;
            return Math.Sqrt(xRange * xRange + yRange * yRange);
        }

        public static double GetAngleTo(this Car car, Point target)
        {
            return car.GetAngleTo(target.X, target.Y);
        }

        public static Point NextPoint(this Car car)
        {
            return new Point(car.X + car.SpeedX, car.Y + car.SpeedY);
        }

        public static Point CurrentPoint(this Car car)
        {
            return new Point(car.X, car.Y);
        }
    }
}
