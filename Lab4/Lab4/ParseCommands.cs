using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class ParseCommands
    {
        public Action<StudentWork> AddWorkAction { get; set; }
        public Action<IEnumerable<StudentWork>> RemoveWorksAction { get; set; }
        public Func<IEnumerable<StudentWork>> GetWorksFunc { get; set; }
        public Action<string> SaveWorks { get; set; }

        private StudentWork? ParseStudentFromCSVString(string CSVString)
        {
            var data = CSVString.Substring(4);
            var parts = data.Split(';');
            if (parts.Length >= 3)
            {
                if (DateTime.TryParse(parts[2], out var date))
                {
                    var sw = new StudentWork(parts[0], parts[1], date);
                    return sw;
                }
            }
            return null;
        }
        public void ProcessCommandFile(string?[] lines)
        {
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmed)) continue;
                if (trimmed.StartsWith("ADD ", StringComparison.OrdinalIgnoreCase))
                {
                    StudentWork student = ParseStudentFromCSVString(trimmed);
                    if (student != null)
                        AddWorkAction?.Invoke(student);
                }
                else if (trimmed.StartsWith("REMOVE ", StringComparison.OrdinalIgnoreCase))
                {
                    var filter = trimmed.Substring(7);
                    if (!string.IsNullOrWhiteSpace(filter))
                        RemoveStudentWorks(filter);
                }
                else if (trimmed.StartsWith("SAVE ", StringComparison.OrdinalIgnoreCase))
                {
                    var filePath = trimmed.Substring(5).Trim();
                    if (string.IsNullOrWhiteSpace(filePath))
                    {
                        Logger.Error("SAVE: путь к файлу не указан");
                        continue;
                    }
                    SaveWorks.Invoke(filePath);
                }
                else 
                    Logger.Error($"Неизвестная команда: {line}");
            }
        }

        private void RemoveStudentWorks(string cond)
        {
            string[] conditions = cond.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            var allworks = GetWorksFunc?.Invoke() ?? Enumerable.Empty<StudentWork>();
            if (conditions.Length == 2)
            {
                if (conditions[0] == "Name")
                {
                    var worksToRemove = allworks.Where(w => w.Name == conditions[1]).ToList();
                    RemoveWorksAction.Invoke(worksToRemove);
                }
                    

                else if (conditions[0] == "NameOfWork")
                {
                    var worksToRemove = allworks.Where(w => w.NameOfWork == conditions[1]).ToList();
                    RemoveWorksAction.Invoke(worksToRemove);
                }

                else if (conditions[0] == "DateOfWork")
                {
                    if (DateTime.TryParse(conditions[1], out var d))
                    {
                        var worksToRemove = allworks.Where(w => w.DateOfWork == d);
                        RemoveWorksAction.Invoke(worksToRemove);
                    }
                    else
                    {
                        Logger.Error($"REMOVE: некорректная дата: {conditions[1]}");
                    }
                }
            }
        }
    }
}
