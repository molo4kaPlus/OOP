using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sharprompt;
using static LR3.LR3;
/*
 * программа выполняет все задачи, прописанные в документе.
 * 1. Упорядовачивание происходит каждую итерацию меню.
 * 2. Вывод первых 4 фигур выполняется через меню, "Show first 4 elements".
 * 3. Вывод 3 последних фигур выполняется по очереди (в случае выхода фигуры за рамки "поля", не рисует ее).
 *    Координатная плоскость размером 30 на 30.
 * 4. Фигуры упорядочиваются с каждой итерацией меню sharpromt.
 * 5. Чтение\запись JSON требует ввода названия файла, который нужно создать\открыть. 
 * 6. В случае ошибки чтения\записи JSON, выводит в консоль исключение.
*/

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
        [JsonDerivedType(typeof(Square), "Square")]
        [JsonDerivedType(typeof(Ellipse), "Ellipse")]
        [JsonDerivedType(typeof(Circle), "Circle")]
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
            public override double SpaceNB { get { return _Width * _Height; } }
            public override double SpaceB { get { return (_Width + _Thickness) * (_Height + _Thickness); } }
        }
        public class Square : Figure
        {
            public Square() { }
            public Square(int sides, int thickness) 
            {
                _FigureType =FigureType.square;
                _Height = sides;
                _Width = sides;
                _Thickness = thickness;
            }
            public override double SpaceNB { get { return _Width * _Height; } }
            public override double SpaceB { get { return (_Width + _Thickness) * (_Height + _Thickness); } }
        }
        public class Ellipse : Figure
        {
            public Ellipse() { }
            public Ellipse(int height,  int width, int thickness)
            {
                _FigureType=FigureType.ellipse;
                _Height = height;
                _Width = width;
                _Thickness = thickness;
            }
            public override double SpaceNB { get { return (3.1416 * (_Height / 2) * (_Width / 2)); } }
            public override double SpaceB { get { return (3.1416 * ((_Height + _Thickness) / 2) * ((_Width + _Thickness) / 2)); } }
        }
        public class Circle : Figure
        {
            public Circle() { }
            public Circle(int radius,  int thickness)
            {
                _FigureType = FigureType.circle;
                _Height = radius;
                _Width = thickness;
                _Thickness = thickness;
            }
            public override double SpaceB { get { return (1.1416 * (_Height * _Height)); } }
            public override double SpaceNB { get { return (1.1416 * (_Height + _Thickness) * (_Height + _Thickness)); } }
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
            public int Lenght()
            {
                return _figures.Count;
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
            public static GraphicRedactor FromJson(string filename)
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
        public static void DrawFigure(Figure figure)
        {
            if (figure.Width > 15 | figure.Height > 15) { Console.WriteLine("The figure will be our of range, can't draw it!"); return; }
            if (figure.Type == FigureType.rectangle) { DrawRectangle(figure); }
            if (figure.Type == FigureType.square) { DrawSquare(figure); }
            if (figure.Type == FigureType.ellipse) { DrawEllipse(figure); }
            if (figure.Type == FigureType.circle) { DrawCicrle(figure); }

            void DrawCicrle(Figure figure) 
            {
                bool[,] desk = new bool[30, 30];
                int Cx = desk.GetLength(0) / 2;
                int Cy = desk.GetLength(1) / 2;
                for (int i = -desk.GetLength(0); i < desk.GetLength(0); i++)
                {
                    for (int j = -desk.GetLength(1); j < desk.GetLength(1); j++)
                    {
                        if ((i*i) + (j*j) < (figure.Height * figure.Height)) { desk[i + Cx, j + Cy] = true; }
                    }
                }
                //DrawOXY(desk);    //елси надо отаброзить координатные прямые
                DrawDesk(desk);
            }
            void DrawEllipse(Figure figure)
            {
                bool[,] desk = new bool[30, 30];
                int Cx = desk.GetLength(0) / 2;
                int Cy = desk.GetLength(1) / 2;
                for (double i = -Cx; i < Cx; i++)
                {
                    for (double j = -Cy; j < Cy; j++)
                    {
                        if ((Math.Pow(i/figure.Width, 2) + Math.Pow(j/figure.Height , 2)) < 1) { desk[Convert.ToInt32(i) + Cx, Convert.ToInt32(j) + Cy] = true; }
                    }
                }
                //DrawOXY(desk);    //елси надо отаброзить координатные прямые
                DrawDesk(desk);
            }
            void DrawSquare(Figure figure)
            {
                bool[,] desk = new bool[30, 30];
                int Cx = desk.GetLength(0) / 2;
                int Cy = desk.GetLength(1) / 2;
                int x1, x2, y1, y2;
                x1 = Cx - figure.Width / 2;
                y1 = Cy - figure.Height / 2;
                x2 = Cx + figure.Width / 2;
                y2 = Cy + figure.Height / 2;
                for (int i = x1; i <= x2; i++) { desk[y1, i] = true; }
                for (int i = y1; i <= y2; i++) { desk[i, x1] = true; }
                for (int i = x2; i >= x1; i--) { desk[y2, i] = true; }
                for (int i = y2; i >= y1; i--) { desk[i, x2] = true; }

                //DrawOXY(desk);    //елси надо отаброзить координатные прямые
                DrawDesk(desk);
            }
            void DrawRectangle(Figure figure)
            {
                bool[,] desk = new bool [30, 30];
                int Cx = desk.GetLength(0) / 2;
                int Cy = desk.GetLength(1) / 2;
                int x1, x2, y1, y2;

                x1 = Cx - figure.Width/2;
                y1 = Cy - figure.Height/2;
                x2 = Cx + figure.Width/2;
                y2 = Cy + figure.Height/2;

                for (int i = x1; i <= x2; i++) { desk[y1, i] = true; }
                for (int i = y1; i <= y2; i++) { desk[i, x1] = true; }
                for (int i = x2; i >= x1; i--) { desk[y2, i] = true; }
                for (int i = y2; i >= y1; i--) { desk[i, x2] = true; }
                
                //DrawOXY(desk);    //елси надо отаброзить координатные прямые
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
                        if (desk[i, j]) { Console.Write("* "); }
                        else { Console.Write("  "); }
                    }
                    Console.WriteLine();
                }
            }
            void DrawOXY(bool[,] desk)
            {
                int x = desk.GetLength(0);
                int y = desk.GetLength(1);
                for (int i = 0; i < x; i++) { desk[i, y/2] = true; }
                for (int i = 0; i < y; i++) { desk[x/2, i] = true; }
            }
        }
        static void Main()
        {
            bool flag = true;
            string filename = "figures.json";
            string[] menuArgs =
            {
                "Show all elements",
                "Show first 4 elements",
                "Add element",
                "Draw last 3 elements",
                "Serialize to JSON",
                "Deserialize from JSON",
                "Exit"
            };
            string[] menufigures = { "Square", "Rectangle", "Ellipse", "Circle", "I changed my mind, back" };

            var myGraphicRedactor = new GraphicRedactor();
            myGraphicRedactor.Add(new Rectangle(8, 8, 1));
            myGraphicRedactor.Add(new Square(5, 1));
            myGraphicRedactor.Add(new Ellipse(10, 5, 1));
            myGraphicRedactor.Add(new Circle(5, 1));
            myGraphicRedactor.SortBySpaceNoBorder();

            Console.WriteLine("Welcome to the menu!");
            while (flag) 
            {
                var menu = Prompt.Select("Select", menuArgs);
                myGraphicRedactor.SortBySpaceNoBorder();
                if (menu == "Show all elements")
                {
                    for (int i = 0; i < myGraphicRedactor.Lenght(); i++)
                    {
                        Console.WriteLine($"Figure: {myGraphicRedactor.GetFigure(i).Type}, Space (no border): {myGraphicRedactor.GetFigure(i).SpaceNB}, Border thickness: {myGraphicRedactor.GetFigure(i).Thickness}");
                    }
                    Console.WriteLine($"\nTotalSpace = {myGraphicRedactor.TotalSpaceNoBorder}");
                }
                if (menu == "Show first 4 elements") 
                {
                    for (int i = 0; i < 4; i++) 
                    {
                        Console.WriteLine($"Figure: {myGraphicRedactor.GetFigure(i).Type}");
                    }
                }
                if (menu == "Add element")
                {
                    Console.WriteLine("What figure do you want to add?");
                    var subMenuAdd = Prompt.Select("Select", menufigures);
                    if (subMenuAdd == "Square") { Console.WriteLine("Type sides of the square, then thicness of border."); myGraphicRedactor.Add(new Square(Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()))); }
                    if (subMenuAdd == "Rectangle") { Console.WriteLine("Type sides of the rectangle, then thicness of border."); myGraphicRedactor.Add(new Rectangle(Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()))); }
                    if (subMenuAdd == "Ellipse") { Console.WriteLine("Type sides of rectangle, ellipse will be inserted in it. Then thicness of border."); myGraphicRedactor.Add(new Ellipse(Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()))); }
                    if (subMenuAdd == "Circle") { Console.WriteLine("Type radius of the circle, Then thicness of border."); myGraphicRedactor.Add(new Circle(Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()))); }
                }
                if (menu == "Draw last 3 elements")
                {
                    for (int i = myGraphicRedactor.Lenght() - 1; i > myGraphicRedactor.Lenght() - 4; i--)
                    {
                        DrawFigure(myGraphicRedactor.GetFigure(i));
                        Console.WriteLine($"Figure type: {myGraphicRedactor.GetFigure(i).Type}");
                        Console.WriteLine($"Figure Space (No border): {myGraphicRedactor.GetFigure(i).SpaceNB}");
                        Console.WriteLine("Enter to draw next figure...");
                        Console.ReadLine();
                    }
                }
                if (menu == "Serialize to JSON") 
                {
                    Console.WriteLine("Enter file name...");
                    filename = Console.ReadLine();
                    try
                    {
                        myGraphicRedactor.ToJson(filename);
                    }
                    catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                }
                if (menu == "Deserialize from JSON") 
                {
                    Console.WriteLine("Enter file name...");
                    filename = Console.ReadLine();
                    try
                    {
                        var myGraphicRedactorTemp = GraphicRedactor.FromJson(filename);
                        myGraphicRedactor = myGraphicRedactorTemp;
                    }
                    catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                }
                if (menu == "Exit") { flag = false; }
            }
        }
    }
}
