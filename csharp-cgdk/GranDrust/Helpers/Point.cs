using System;

// ReSharper disable once CheckNamespace
namespace GranDrust.Helpers
{
    public struct Point : IEquatable<Point>
    {
        public double X { get; set; }
        public double Y { get; set; }

        private const double TOLERANCE = 0.001;

        public bool Equals(Point other)
        {
            return Math.Abs(X - other.X) < TOLERANCE
                      && Math.Abs(Y - other.Y) < TOLERANCE;
        }

        public override string ToString()
        {
            return $"{X},{Y}";
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
