using System;

namespace VoronoiLib.Structures
{
    [Serializable]
    public class VPoint
    {
        public double X { get; }
        public double Y { get; }

        internal VPoint() { }
        internal VPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
