using System.Text.Json;
using System.Text.Json.Serialization;
//стандартная библиотка .NET

namespace LR3
{
    class LR3
    {
        public abstract class Figure
        {
            public double Thickness { get; }
            public double Radius { get; }
            public Tuple<double, double> Sides { get; }
            public string FigureType { get; set; }

            public Figure(double _radius, double _thickness) //Circle
            {
                Thickness = _thickness;
                Radius = _radius;
            }
            public Figure(Tuple<double, double> _sides, double _thickness) //Square/rectangle/ellipse
            {
                Thickness = _thickness;
                Sides = _sides;
            }
            public abstract double GetSpace();
        }

        class Square : Figure
        {
            public Square(double Thickness, Tuple<double, double> Sides) : base(Thickness, Sides) => FigureType = "Square";
        }

        static void Main()
        {
            
        }
    }
}
