using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
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
        [XmlInclude(typeof(Rectangle))]
        public abstract class Figure
        {
            protected FigureType _FigureType;
            protected Figure() { }
            protected Figure(FigureType type)
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
        }
        public class Rectangle : Figure
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public double Thickness { get; set; }
            public Rectangle() { }
            public Rectangle(FigureType type, int width, int height, double thickness)
                :base (type)
            {
                Width = width;
                Height = height;
                Thickness = thickness;
            }
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
            }
            public void SortBySpaceNoBorder()
            {
                _figures.Sort(new BySpaceComparerNoBorder());
            }
            public void ToXML (string filename)
            {
                var serializer = new XmlSerializer(typeof(List<Figure>));
                using (var stream = File.OpenWrite(filename))
                {
                    serializer.Serialize(stream, _figures);
                    stream.Flush();
                }
            }
            public static GraphicRedactor FromXML (string filename)
            {
                var graphicRedactor = new GraphicRedactor();
                var serializer = new XmlSerializer (typeof(List<Figure>));

                using (var stream = File.OpenRead(filename))
                {
                    var figures = serializer.Deserialize(stream) as IEnumerable<Figure>;
                    if (figures != null) { graphicRedactor._figures.AddRange(figures); }
                }

                return graphicRedactor;
            }
        }

        static void Main()
        {
            List<Figure> figures123 = new List<Figure>();
            var myGraphicRedactor = new GraphicRedactor();

            myGraphicRedactor.Add(new Rectangle(FigureType.square, 2, 3, 0.3));
            myGraphicRedactor.Add(new Rectangle(FigureType.square, 3, 3, 0.5));

            myGraphicRedactor.SortBySpaceNoBorder();

            const string filename = @"c:\users\gleb\xml.xml";
            myGraphicRedactor.ToXML(filename);

            try
            {
                var myGraphicRedactorNew = GraphicRedactor.FromXML(filename);
                foreach (var figure in myGraphicRedactorNew.GetFigures())
                {
                    Console.WriteLine($"Figure: {figure.Type}");
                }
                Console.WriteLine($"\nTotalSpace = {myGraphicRedactorNew.TotalSpaceNoBorder}");
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }
    }
}
