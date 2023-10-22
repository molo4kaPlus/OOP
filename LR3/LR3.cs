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
            protected int _Height;
            protected int _Width;
            protected Figure() { }
            public FigureType Type
            {
                get { return _FigureType; }
                set { _FigureType = value; }
            }
            public int Height
            {
                get { return _Height; }
                set { _Height = value; }
            }
            public int Width
            {
                get { return _Width; }
                set { _Width = value; }
            }
            public abstract double SpaceNB { get; }
            protected Figure(FigureType type)
            {
                Type = type;
            }
        }
        public class Rectangle : Figure
        {
            public Rectangle() { }
            public Rectangle(FigureType type, int height, int width)
            {
                _FigureType = type;
                _Height = height;
                _Width = width;
            }
            public override double SpaceNB
            {
                get { return _Width * _Height; }
            }
        }
        public class GraphicRedactor
        {
            private readonly List<Figure> _figures = new List<Figure>();
            //public double TotalSpaceNoBorder
            //{
            //    get
            //    {
            //        double space = 0;
            //        foreach (var figure in _figures) { space += figure.SpaceNoBorder; }
            //        return space;
            //    }
            //}
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
            //private class BySpaceComparerNoBorder : IComparer<Figure>
            //{
            //    public int Compare(Figure x, Figure y)
            //    {
            //        return x.SpaceNoBorder.CompareTo(y.SpaceNoBorder);  
            //    }
            //}
            //public void SortBySpaceNoBorder()
            //{
            //    _figures.Sort(new BySpaceComparerNoBorder());
            //}
            public void ToJson(string filename)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                using (var stream = File.OpenWrite(filename))
                {
                    JsonSerializer.Serialize(stream, _figures, options);
                }
            }
        }
        //жопа
        static void Main()
        {
            List<Figure> figures123 = new List<Figure>();
            var myGraphicRedactor = new GraphicRedactor();

            myGraphicRedactor.Add(new Rectangle(FigureType.square, 1, 2));
            myGraphicRedactor.Add(new Rectangle(FigureType.square, 2, 1));

            //myGraphicRedactor.SortBySpaceNoBorder();

            const string filename = "json.json";

            myGraphicRedactor.ToJson(filename);

            //myGraphicRedactor.ToXML(filename);

            //try
            //{
            //    var myGraphicRedactorNew = GraphicRedactor.FromXML(filename);
            //    foreach (var figure in myGraphicRedactorNew.GetFigures())
            //    {
            //        Console.WriteLine($"Figure: {figure.Type}");
            //    }
            //    Console.WriteLine($"\nTotalSpace = {myGraphicRedactorNew.TotalSpaceNoBorder}");
            //}
            //catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }
    }
}
