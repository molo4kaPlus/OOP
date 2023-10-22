using System.Text.Json;
using System.Text.Json.Serialization;
//стандартная библиотка .NET

namespace LR3
{
    class LR3
    {
        public enum FigureType
        {
            None,
            ellipse,
            circle,
            square,
            rectangle
        }
        public abstract class Figure
        {
            protected FigureType _FigureType;
            protected Figure() { }
            protected Figure(FigureType type, double thickness)
            {
                Type = type;
            }
            public FigureType Type
            {
                get { return _FigureType; }
                set
                {
                    _FigureType = value;
                }
            }
            public abstract double SpaceNoBorder { get; }
            public abstract double SpaceBorder { get; }
            public double Thickness { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Radius { get; set; }
        }
        public class Rectangle : Figure
        {
            public Rectangle() { }
            public override double SpaceNoBorder
            {
                get { return Width * Height; }
            }
            public override double SpaceBorder
            {
                get { return (Width + Thickness) * (Height + Thickness); }
            }
        }
        public class GraphicRedactor
        {
            private readonly List<Figure> _figures = new List<Figure>();
            public double TotalSpaceNoBorder
            {
                get
                {
                    double space = 0;
                    foreach (var figure in _figures) { space += figure.SpaceNoBorder; }
                    return space;
                }
            }
            public void Add(Figure figure)
            {
                if (figure == null || figure.Type == FigureType.None)
                {
                    throw new ArgumentException(nameof(figure));
                }
                _figures.Add(figure);
            }
            public IEnumerable<Figure> GetFigures()
            {
                return _figures;
            }
            private class BySpaceComparerNoBorder : IComparer<Figure>
            {
                public int Compare(Figure x, Figure y)
            {
                    return x.SpaceNoBorder.CompareTo(y.SpaceNoBorder);  
            }
            public abstract double GetSpace();
        }
            public void SortBySpaceNoBorder()
        {
                _figures.Sort(new BySpaceComparerNoBorder());
            }
        }

        static void Main()
        {
            
        }
    }
}
