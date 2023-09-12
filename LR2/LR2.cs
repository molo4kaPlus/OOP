using Sharprompt;
using System.Collections.Generic;
using System.Numerics;

namespace LR2
{
    public class array
    {
        public List<int>? _array;

        public override string ToString()
        {
            return $"{_array}";
        }
    }

    class LR2
    {
        static void Main()
        {
            bool flag = true;
            string[] menuArguments =
            {
                "Show array A",
                "Add a number",
                "Exit"
            };

            array arrayA = new array { _array = { 1, 2, 3} };

            Console.WriteLine("Welcome to the menu!");
            while (flag == true)
            {
                var menu = Prompt.Select("Select", menuArguments);
                if (menu == "Show array A") { Console.WriteLine(arrayA._array); }
                if (menu == "Exit") { flag = false; }
            }
        }
    }
}