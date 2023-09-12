using Sharprompt;
using System.Collections.Generic;
using System.Numerics;

namespace LR2
{
    public class array
    {
        private readonly int[] _array;
        public array(params int[] array) { _array = array;  }
        //public array(int[] array, int number) { array.Append(number);  _array = array; }
        public override string ToString()
        {
            string str = "";
            foreach (int num in _array)
            {
                str += num.ToString() + " ";
            }
            return $"{str}";
        }
        public array Add(int ToAdd)
        {
            _array.Append(ToAdd);
            Console.WriteLine(_array.ToString());
            return new array(_array);
        }

        public static array operator +(array arrayA, array arrayB)
        {
            return new array(0);
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
                "Show array B",
                "Add a number",
                "Exit"
            };

            array arrayA = new array(1, 2, 3);
            array arrayB = new array(2, 3, 4);
            arrayA.Add(5);
            arrayB.Add(6);

            Console.WriteLine("Welcome to the menu!");
            while (flag == true)
            {
                var menu = Prompt.Select("Select", menuArguments);
                if (menu == "Show array A") { Console.WriteLine(arrayA.ToString()); }
                if (menu == "Show array B") { Console.WriteLine(arrayB.ToString()); }
                if (menu == "Show array A and B") { Console.WriteLine(arrayA.ToString()); Console.WriteLine(arrayB.ToString); }
                if (menu == "Exit") { flag = false; }
            }
        }
    }
}