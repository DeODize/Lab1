using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Lab4
{
    static public class ParseTextFile
    {
        public static void LoadObjects(List<StudentWork> newWorks, ref ObservableCollection<StudentWork> oldWorks, ref bool isOpen)
        {
            if (!isOpen)
            {
                var _tempWorks = new List<StudentWork>(oldWorks);
                oldWorks.Clear();
                foreach (var obj in newWorks)
                    oldWorks.Add(obj);
                foreach (var obj in _tempWorks)
                    oldWorks.Add(obj);
                isOpen = true;
            }
            else
            {
                oldWorks.Clear();
                foreach (var obj in newWorks)
                    oldWorks.Add(obj);
            }
        }

        public static List<StudentWork> FileParser(string[] stringsFromFile)
        {
            if (stringsFromFile != null)
            {
                var fileWorks = new List<StudentWork>();
                foreach (var line in stringsFromFile)
                {
                    string? trimmed = line.Trim();
                    StudentWork part = ParseLine(trimmed);
                    if (part != null)
                        fileWorks.Add(part);
                    else
                        try { Logger.Error($"Не удалось распарсить строку: '{trimmed}'"); } catch { }
                }
                return fileWorks;
            }
            else return null;
        }


        public static StudentWork ParseLine(string rawString)
        {
            try
            {
                List<string> list = new List<string>();
                string[] words = rawString.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string word in words)
                {
                    string[] subword = word.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (subword.Length != 0) list.Add(word);
                }
                StudentWork studentWork = new StudentWork(list[0], list[1], DateTime.Parse(list[2]));
                return studentWork;
            }
            catch
            {
                return null;
            }
        }

        public static ObservableCollection<StudentWork> Filter(ObservableCollection<StudentWork> works, string filter)
        {
            ObservableCollection<StudentWork> filteredWorks = new ObservableCollection<StudentWork>();
            foreach (StudentWork studentWork in works)
            {
                if (studentWork.ToRawString().Contains(filter))
                    filteredWorks.Add(studentWork);
            }
            return filteredWorks;
        }
        public static List<StudentWork> Filter(List<StudentWork> works, string filter)
        {
            List<StudentWork> filteredWorks = new List<StudentWork>();
            foreach (StudentWork studentWork in works)
            {
                if (studentWork.ToRawString().Contains(filter))
                    filteredWorks.Add(studentWork);
            }
            return filteredWorks;
        }
        
    }
}
