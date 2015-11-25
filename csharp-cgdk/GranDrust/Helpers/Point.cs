using System;

// ReSharper disable once CheckNamespace
namespace GranDrust.Helpers
{
    public struct Point : IEquatable<Point>
    {
        private double _x;

        public double X
        {
            get { return _x; }
            private set { _x = value; }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            private set { _y = value; }
        }

        private const double TOLERANCE = 0.001;

        public bool Equals(Point other)
        {
            return Math.Abs(X - other.X) < TOLERANCE
                      && Math.Abs(Y - other.Y) < TOLERANCE;
        }

        public override string ToString()
        {
            return String.Format("{0},{1}", X, Y);
        }

        public Point(double x, double y)
        {
            _x = x;
            _y = y;
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
