using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;

namespace Lab4
{
    static public class Logger
    {
        private static string _filePath = "app.log";

        delegate void Message(string hehe);

        //public static Message? mes;
        public static Action<string> OnLog; // подписка UI

        public static void Init(string filePath, Action<string> logAction)
        {
            _filePath = filePath;
            OnLog = logAction;
        }

        public static void Error(string message)
        {
            Write("ERROR", message);
        }

        private static void Write(string level, string message)
        {
            string log = $"{DateTime.Now:HH:mm:ss} [{level}] {message}";
            try
            {
                File.AppendAllText(_filePath, log + Environment.NewLine);
            }
            catch { }
            OnLog?.Invoke(log);
        }
    }
}
