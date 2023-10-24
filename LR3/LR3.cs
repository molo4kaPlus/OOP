using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using static LR3.LR3;
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
        [JsonDerivedType(typeof(Rectangle), "Rectangle")]
        public abstract class Figure
        {
            protected FigureType _FigureType;
            protected int _Height;
            protected int _Width;
            protected int _Thickness;
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
            public int Thickness
            {
                get { return _Thickness; }
                set { _Thickness = value; }
            }
            public abstract double SpaceNB { get; }
            public abstract double SpaceB { get; }
            protected Figure(FigureType type)
            {
                Type = type;
            }
        }
        public class Rectangle : Figure
        {
            public Rectangle() { }
            public Rectangle(int height, int width, int thickness)
            {
                _FigureType = FigureType.rectangle;
                _Height = height;
                _Width = width;
                _Thickness = thickness;
            }
            public override double SpaceNB
            {
                get { return _Width * _Height; }
            }
            public override double SpaceB
            {
                get { return (_Width + _Thickness) * (_Height + _Height); }
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
                    foreach (var figure in _figures) { space += figure.SpaceNB; }
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
            public Figure GetFigure(int num)
            {
                return _figures[num];
            }
            public IEnumerable<Figure> GetFigures()
            {
                return _figures;
            }
            private class BySpaceComparerNoBorder : IComparer<Figure>
            {
                public int Compare(Figure x, Figure y)
                {
                    if (x.SpaceNB == y.SpaceNB) { return x.Thickness.CompareTo(y.Thickness); }
                    return x.SpaceNB.CompareTo(y.SpaceNB);
                }
            }
            public void SortBySpaceNoBorder()
            {
                _figures.Sort(new BySpaceComparerNoBorder());
            }
            public void ToJson(string filename)
            {
                var options = new JsonSerializerOptions { WriteIndented = true, /*Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }*/ };
                using (var stream = File.OpenWrite(filename))
                {
                    JsonSerializer.Serialize(stream, _figures, options);
                }
            }
            public static GraphicRedactor? FromJson(string filename)
            {
                var temp = new GraphicRedactor();
                List<Figure> figures;
                var options = new JsonSerializerOptions { WriteIndented = true, /*Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }*/ };
                using (var stream = File.OpenRead(filename))
                {
                    figures = JsonSerializer.Deserialize<List<Figure>>(stream, options);
                }
                if (figures != null) { temp._figures.AddRange(figures);}
                else { return default; }
                return temp;
            }
        }
        //TODO
        public static void DrawFigure(Figure figure)
        {
            if (figure.Type == FigureType.rectangle) { DrawRectangle(figure); }

            void DrawRectangle(Figure figure)
            {
                bool[,] desk = new bool [50, 100];
                DrawOXY(desk);
                DrawDesk(desk);
            }

            void DrawDesk(bool[,] desk)
            {
                int x = desk.GetLength(0);
                int y = desk.GetLength(1);
                for (int i = 0; i < x; i++) 
                {
                    for (int j = 0;  j < y; j++)
                    {
                        if (desk[i, j]) { Console.Write("*"); }
                        else { Console.Write(" "); }
                    }
                    Console.WriteLine();
                }
            }
            void DrawOXY(bool[,] desk)
            {
                int x = desk.GetLength(0);
                int y = desk.GetLength(1);
                for (int i = 0; i < x; i++) { desk[i, y/2] = true; }
            }
        }
        //TODO
        public class Menu
        {
            string[] menuArgs =
            {
                
            };

        }
        //жопа
        static void Main()
        {
            var myGraphicRedactor = new GraphicRedactor();

            myGraphicRedactor.Add(new Rectangle(1, 2, 1));
            myGraphicRedactor.Add(new Rectangle(2, 2, 2));
            myGraphicRedactor.Add(new Rectangle(2, 2, 1));
            myGraphicRedactor.Add(new Rectangle(3, 3, 1));

            myGraphicRedactor.SortBySpaceNoBorder();

            const string filename = "json.json";
            myGraphicRedactor.ToJson(filename);
            //myGraphicRedactor.ToXML(filename);
            DrawFigure(myGraphicRedactor.GetFigure(0));

            try
            {
                var myGraphicRedactorNew = GraphicRedactor.FromJson(filename);
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
