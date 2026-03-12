using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    static class ParseFile
    {
        public static string SaveDialog()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Text files|*.txt|All files|*.*";
            if (dlg.ShowDialog() == true)
                return dlg.FileName;
            return null;
        }

        public static bool WriteFile(string filePath, List<StudentWork> works)
        {
            try
            {
                var lines = works.Select(w => w.ToRawString()).ToArray();
                System.IO.File.WriteAllLines(filePath, lines, System.Text.Encoding.UTF8);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string OpenDialog()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Text files|*.txt|All files|*.*";
            if (dlg.ShowDialog() == true)
                return dlg.FileName;
            return null;
        }

        public static List<StudentWork> ReadFile(string filePath)
        {
            System.Collections.ObjectModel.ObservableCollection<StudentWork> works = new();
            try
            {
                var lines = System.IO.File.ReadAllLines(filePath, System.Text.Encoding.UTF8);
                var fileWorks = new List<StudentWork>();
                foreach (var line in lines)
                {
                    var trimmed = line.Trim();
                    var part = ParseLine(trimmed);
                    if (part != null)
                        fileWorks.Add(part);
                    else return null;
                }
                return fileWorks;
            }
            catch
            {
                return null;
            }
        }

        private static StudentWork ParseLine(string rawString)
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

        public static ObservableCollection<StudentWork> Filter(List<StudentWork> works, string filter)
        {
            ObservableCollection<StudentWork> filteredWorks = new ObservableCollection<StudentWork>();
            foreach (StudentWork studentWork in works)
            {    
                if (studentWork.ToRawString().Contains(filter))
                    filteredWorks.Add(studentWork);
            }
            return filteredWorks;
        }


    }
}
