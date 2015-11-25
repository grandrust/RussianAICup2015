using System;
using System.CodeDom;

// ReSharper disable once CheckNamespace
namespace GranDrust.Helpers
{
    public struct Point : IEquatable<Point>
    {
        public double X;
        public double Y;

        private const double TOLERANCE = 0.001;

        public bool Equals(Point other)
        {
            return Math.Abs(X - other.X) < TOLERANCE
                      && Math.Abs(Y - other.Y) < TOLERANCE;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", X, Y);
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point && Equals((Point) obj);
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
