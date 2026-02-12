using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    internal class ClassStudentWork
    {
        public string Name { get; set; }
        public string NameOfWork { get; set; }
        public DateTime DateOfWork { get; set; }
        public void DisplayInfo()
        {
            Console.WriteLine($"Студент: {Name}");
            Console.WriteLine($"Название работы: \"{NameOfWork}\" ");
            Console.WriteLine($"Дата выдачи: {DateOfWork.ToShortDateString()}");
        }
        public ClassStudentWork(string name, string nameOfWork, DateTime date)
        {
            Name = name;
            NameOfWork = nameOfWork;
            DateOfWork = date;
        }
        ~ClassStudentWork()
        {
        }
    }
}
