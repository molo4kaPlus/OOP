using Sharprompt;
using System.Numerics;

namespace LR1
{
    struct date
    {
        public int year;
        public int month;
        public int day;
    }
    struct student
    {
        public int number;
        public string name;
        public string surname;
        public date birthdate;
    }



    public class group
    {
        public void dosomething() { Console.WriteLine("jopa"); }

        public void displayStudents()
        {

        }

        public void addStudent()
        {
            Console.WriteLine("adding new student...");
        }
    }

    class LR1
    {
       static void Main()
        {
            bool flag = true;
            student[] students; 

            Console.WriteLine("Hello! Welcome to the group manager!");

            while (flag == true)
            {
                var city = Prompt.Select("Select", new[] { "dosomething", "add student", "exit" });
                var group = new group();
                if (city == "dosomething") { group.dosomething(); }
                if (city == "add student") { group.addStudent(); }
                if (city == "exit") { flag = false; }
            }
        }
    }
}
