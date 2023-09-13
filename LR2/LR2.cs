using Sharprompt;
using System.Collections.Generic;
using System.Numerics;

namespace LR2
{
    public class array
    {
        private readonly List<int> _array;
        public array(List<int> array) { _array = array;  }
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
            _array.Add(ToAdd);
            return new array(_array);
        }

        public array Remove(int ToRemove) 
        {

            return new array(_array);
        }

        //public static array operator +(array arrayA, array arrayB)
        //{
        //    return new array(0);
        //}
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
                "Add a number to array A",
                "Add a number to array B",
                "Exit"
            };

            array arrayA = new array(new List<int> { 1, 2, 3 });
            array arrayB = new array(new List<int> { 3, 2, 1 });

            Console.WriteLine("Welcome to the menu!");
            while (flag == true)
            {
                var menu = Prompt.Select("Select", menuArguments);
                if (menu == "Show array A") { Console.WriteLine(arrayA.ToString()); }
                if (menu == "Show array B") { Console.WriteLine(arrayB.ToString()); }
                if (menu == "Add a number to array A") { arrayA.Add(Convert.ToInt32(Console.ReadLine())); }
                if (menu == "Add a number to array B") { arrayB.Add(Convert.ToInt32(Console.ReadLine())); }
                //if (menu == "Show array A and B") { Console.WriteLine(arrayA.ToString()); Console.WriteLine(arrayB.ToString); }
                if (menu == "Exit") { flag = false; }
            }
        }
    }
}