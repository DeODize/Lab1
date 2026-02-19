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
        static string trueString(string s)
        {
            string[] strings = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string result = "";
            for(int i = 0; i < strings.Length; i++)
            {
                result += strings[i];
                if (i != strings.Length - 1) result += " ";
            }
            return result;
        }


        static void Main(string[] args)
        {
            string rawString, name, work;
            DateTime dateOfWork;

            Console.WriteLine("Введите данные о теме работы студента в следующем формате:");
            Console.WriteLine("\"Имя студента (строка)\" \"название темы (строка)\" дата выдачи");
            rawString = "    \" Ivanov Ivan\"           \"IS Decanat\"   23.12.2023";// Console.ReadLine();
            List<string> list = new List<string>();
            string[] words = rawString.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words) 
            {
                string[] subword = word.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (subword.Length != 0) list.Add(word);
            }

            name = trueString(list[0]);
            work = trueString(list[1]);

            words = list[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            dateOfWork = DateTime.Parse(words[0]);

            ClassStudentWork studentWork = new ClassStudentWork(name, work, dateOfWork);
            studentWork.DisplayInfo();
        }
    }
}
