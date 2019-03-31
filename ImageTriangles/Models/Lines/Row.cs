using Enums;

namespace Models.Lines
{
    public class Row : Line
    {
        public RowOrder RowOrder { get; set; }
        public float Height { get; set; }
    }
}