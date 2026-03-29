using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    static class FileHandler
    {
        public static string SaveDialog()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Text files|*.txt|All files|*.*";
            if (dlg.ShowDialog() == true)
                return dlg.FileName;
            return null;
        }

        public static string OpenDialog()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
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

        public static string[]? ReadFile(string filePath)
        {
            try
            {
                var lines = System.IO.File.ReadAllLines(filePath, System.Text.Encoding.UTF8);
                return lines;
            }
            catch
            {
                return null;
            }
        }
    }
}
