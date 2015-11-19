// ReSharper disable once CheckNamespace
namespace GranDrust.Helpers
{
    public struct Point
    {
        public double X;
        public double Y;

        public override string ToString()
        {
            return string.Format("{0},{1}", X, Y);
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
