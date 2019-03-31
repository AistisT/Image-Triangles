using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Triangle
    {
        public Triangle(int v1X, int v1Y, int v2X, int v2Y, int v3X, int v3Y, string name)
        {
            Point1 = new Point
            {
                X = v1X,
                Y = v1Y
            };
            Point2 = new Point
            {
                X = v2X,
                Y = v2Y
            };
            Point3 = new Point
            {
                X = v3X,
                Y = v3Y
            };
            Name = name;
        }

        public Triangle()
        {
            Point1 = new Point();
            Point2 = new Point();
            Point3 = new Point();
        }
        public Triangle(Point point1, string name, Triangle triangle)
        {
            Point1 = point1;
            Point2 = triangle.Point2;
            Point3 = triangle.Point3;
            Name = name;
        }

        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public Point Point3 { get; set; }
        [Required]
        public string Name { get; set; }

    }
}