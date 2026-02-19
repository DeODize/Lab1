using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string rawString, name, work;
            DateTime dateOfWork;

            Console.WriteLine("Введите данные о теме работы студента в следующем формате:");
            Console.WriteLine("\"Имя студента (строка)\" \"название темы (строка)\" дата выдачи");
            rawString = Console.ReadLine();

            string[] words = rawString.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries);

            name = words[0];
            work = words[2];

            words = words[3].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            dateOfWork = DateTime.Parse(words[0]);

            ClassStudentWork studentWork = new ClassStudentWork(name, work, dateOfWork);
            studentWork.DisplayInfo();
        }
    }
}
