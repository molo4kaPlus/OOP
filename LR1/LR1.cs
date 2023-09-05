using Sharprompt;
using System.Collections.Generic;
using System.Numerics;

namespace LR1
{
    public class student 
    {
        public string phone { get; set; }
        public string fullName { get; set; }
        public DateOnly birthDate { get; set; }
        public override string ToString()
        {
            return $"({fullName}) " + $"{birthDate:d}, " + $"{phone ?? "null"}";
        }
    }

    public class studentList
    {
        private readonly List<student> _students = new List<student>();
        public void displayAllStudents()
        {
            for (int i = 0; i < _students.Count; i++)
            {
                Console.WriteLine(_students[i]);
            }
        }

        public void addStudentFromCode(student student)
        {
            if (student == null) { throw new ArgumentNullException(nameof(student)); }
            if (string.IsNullOrEmpty(student.fullName)) { throw new ArgumentNullException(nameof(student.fullName)); }
            _students.Add(student);
        }

        public void addStudentFromConsole()
        {
            DateOnly date;
            Console.WriteLine("Enter full name...");
            string fullName = Console.ReadLine();
            Console.WriteLine("Enter birth date (YMD)...");
            try { date = new DateOnly(Convert.ToInt16(Console.ReadLine()), Convert.ToInt16(Console.ReadLine()), Convert.ToInt16(Console.ReadLine())); }
            catch (Exception ex) { Console.WriteLine("Wrong entered YMD, returning to menu..."); return; }
            Console.WriteLine("Enter phone number...");
            string phone = Console.ReadLine();

            student temp = new student
            {
                fullName = fullName,
                birthDate = date,
                phone = phone
            };

            if (temp == null) { throw new ArgumentNullException(nameof(student)); }
            if (string.IsNullOrEmpty(temp.fullName)) { throw new ArgumentNullException(nameof(student.fullName)); }
            _students.Add(temp);
        }

        public void deleteStudent()
        {
            Console.WriteLine("Welcome to the delete menu!");
            var deleteMenu = Prompt.Select("Select", _students);
            _students.Remove(deleteMenu); 
        }

        private class BirthDateComparer : Comparer<student>
        {
            public override int Compare(student? x, student? y)
            {
                return x.birthDate.CompareTo(y.birthDate);
            }
        }

        // sorting by birth date
        public void displaySortedStudents()
        {
            if (_students.Count == 0) return;
            _students.Sort(new BirthDateComparer());
            Console.WriteLine("Done!");
        }

        public void searchStudentByNumber()
        {
            Console.WriteLine("Enter number to find...");
            string item = Console.ReadLine();
            int index = _students.FindIndex(a => a.phone == item);
            if (index != -1)
            {
                Console.WriteLine(_students[index]);
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }
    }

    class LR1
    {
       static void Main()
        {
            bool flag = true;
            student[] students; 

            Console.WriteLine("Hello! Welcome to the group manager!");

            var list = new studentList();

            list.addStudentFromCode(new student
            {
                fullName = "Кузнецов Генадий Глебович",
                birthDate = new DateOnly(2005, 3, 12),
                phone = "79518384664"
            });
            list.addStudentFromCode(new student
            {
                fullName = "Кузьмин Глеб Олегович",
                birthDate = new DateOnly(2000, 12, 8),
                phone = "795181815625"
            });
            list.addStudentFromCode(new student
            {
                fullName = "Жмышенко Альберт Ахмедович",
                birthDate = new DateOnly(2003, 5, 8),
                phone = "88005556565"
            });

            while (flag == true)
            {
                var menu = Prompt.Select("Select", new[] { "Display all students", "Delete student", "Add student", "Find student by phone", "Sort students", "exit" });
                if (menu == "Display all students") { list.displayAllStudents(); }
                if (menu == "Add student") { list.addStudentFromConsole(); }
                if (menu == "Delete student") { list.deleteStudent(); }
                if (menu == "Find student by phone") { list.searchStudentByNumber(); }
                if (menu == "Sort students") { list.displaySortedStudents(); }
                if (menu == "exit") { flag = false; }
            }
        }
    }
}
