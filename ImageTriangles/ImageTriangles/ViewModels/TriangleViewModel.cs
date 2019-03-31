using Models;

namespace ImageTriangles.ViewModels
{
    public class TriangleViewModel : Triangle
    {
        public TriangleViewModel()
        {
            
        }
        public TriangleViewModel(Triangle triangle)
        {
            Point1 = triangle.Point1;
            Point2 = triangle.Point2;
            Point3 = triangle.Point3;
            Name = triangle.Name;
        }
        public bool? TriangleFound { get; set; } = null;
    }
}