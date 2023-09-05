using Sharprompt;
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
            Console.WriteLine("Enter full name,birthdate(YMD), phone number...");
            student temp = new student
            {
                fullName = Console.ReadLine(),
                birthDate = new DateOnly(
                    Convert.ToInt32(Console.ReadLine()),
                    Convert.ToInt32(Console.ReadLine()),
                    Convert.ToInt32(Console.ReadLine())),
                phone = Console.ReadLine()
            };

            if (temp == null) { throw new ArgumentNullException(nameof(student)); }
            if (string.IsNullOrEmpty(temp.fullName)) { throw new ArgumentNullException(nameof(student.fullName)); }
            _students.Add(temp);
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
                fullName = "Кузьмин Глеб Олегович",
                birthDate = new DateOnly(2000, 12, 8),
                phone = "795181815625"
            });
            list.addStudentFromCode(new student
            {
                fullName = "Жмышенко Альберт АХмедович",
                birthDate = new DateOnly(2003, 5, 8),
                phone = "88005556565"
            });

            while (flag == true)
            {
                var menu = Prompt.Select("Select", new[] { "Add new student", "Display all students", "exit" });
                if (menu == "Add new student") { list.addStudentFromConsole(); }
                if (menu == "Display all students") { list.displayAllStudents(); }
                if (menu == "exit") { flag = false; }
            }
        }
    }
}
