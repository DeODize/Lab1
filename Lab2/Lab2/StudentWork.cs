using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Lab2
{
    public class StudentWork
    {
        public string Name { get; set; }
        public string NameOfWork { get; set; }
        public DateTime DateOfWork { get; set; }
        public string ToRawString()
        {
            var datePart = DateOfWork == DateTime.MinValue ? string.Empty : DateOfWork.ToString("yyyy-MM-dd");
            return $"\"{Name}\" \"{NameOfWork}\" {datePart}".Trim();
        }
        public StudentWork(string name, string nameOfWork, DateTime date)
        {
            Name = name;
            NameOfWork = nameOfWork;
            DateOfWork = date;
        }
        ~StudentWork()
        {
        }
    }
}
