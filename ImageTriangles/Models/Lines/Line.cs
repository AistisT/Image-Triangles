namespace Models.Lines
{
    public class Line
    {
        protected Line()
        {
            StarPoint = new Point();
            EndPoint = new Point();
        }

        public Point StarPoint { get; set; }
        public Point EndPoint { get; set; }
    }
}