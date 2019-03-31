using System;

namespace Models
{
    public class Point
    {
        public Point()
        {

        }
        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public override bool Equals(object obj)
        {
            bool equals = false;
            if (obj is Point point)
            {
                equals = Equals(point);
            }

            return equals;
        }

        protected bool Equals(Point other)
        {
            return Math.Abs(X - other.X) < 0.0001 && Math.Abs(Y - other.Y) < 0.0001;
        }
    }
}