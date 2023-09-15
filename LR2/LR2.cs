using Sharprompt;
using System.Collections.Generic;
using System.Numerics;

namespace LR2
{
    public class array
    {
        private readonly List<int> _array;
        public array(List<int> array) { _array = array;  }

        // Выводим множество как строку.
        public override string ToString()
        {
            string str = "";
            foreach (int num in _array)
            {
                str += num.ToString() + " ";
            }
            return $"{str}";
        }

        // Добавляем элемент в множество.
        public array Add(int ToAdd)
        {
            _array.Add(ToAdd);
            return new array(_array);
        }

        // Удаляем элемент из множества.
        public array Remove(int target) 
        {
            _array.RemoveAt(_array.IndexOf(target));
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
                "Remove a number from array A",
                "Remove a number from array B",
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
                if (menu == "Add a number to array A") { Console.WriteLine("Enter a number to add..."); arrayA.Add(Convert.ToInt32(Console.ReadLine())); }
                if (menu == "Add a number to array B") { Console.WriteLine("Enter a number to add..."); arrayB.Add(Convert.ToInt32(Console.ReadLine())); }
                if (menu == "Remove a number from array A") { Console.WriteLine("Enter a number to remove..."); arrayA.Remove(Convert.ToInt32(Console.ReadLine())); }
                if (menu == "Remove a number from array B") { Console.WriteLine("Enter a number to remove..."); arrayB.Remove(Convert.ToInt32(Console.ReadLine())); }
                if (menu == "Exit") { flag = false; }
            }
        }
    }
}