using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace Lab3.Tests;

[TestClass]
public sealed class Test1
{
    [DataTestMethod]
    [DataRow("\"Bob\" \"Some work\" 2012-12-12", "Bob", "Some work", "2012-12-12")]
    [DataRow("  \"Jill\"      \"Forest camp\"             2023-02-14", "Jill", "Forest camp", "2023-02-14")]
    public void StudentWorkParseFromString(string studentString, string nameTrueStudent, string workTrueStudent, string dateTrueStudent)
    {
        Lab3.StudentWork student = Lab3.ParseFile.ParseLine(studentString);
        Lab3.StudentWork trueStudent = new StudentWork(nameTrueStudent, workTrueStudent, DateTime.Parse(dateTrueStudent));

        Assert.AreEqual(trueStudent.Name, student.Name);
        Assert.AreEqual(trueStudent.NameOfWork, student.NameOfWork);
        Assert.AreEqual(trueStudent.DateOfWork, student.DateOfWork);
    }

    [DataTestMethod]
    [DataRow("this is an invalid string")]
    [DataRow("\"Bob\" \"Some work\" invalid date")]
    [DataRow(null)]
    public void ParseLineInvalidReturnsNull(string? rawString)
    {
        var result = Lab3.ParseFile.ParseLine(rawString);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void StudentWorkParsingFromStrings()
    {
        string[] rawStrings = { "\"Bob\" \"Some work\" 2012-12-12", "  \"Jill\"      \"Forest camp\"             2023-02-14" };
        List<Lab3.StudentWork> students = Lab3.ParseFile.FileParser(rawStrings);
        Lab3.StudentWork firststudent = new StudentWork("Bob", "Some work", DateTime.Parse("2012-12-12"));
        Lab3.StudentWork secondStudent = new StudentWork("Jill", "Forest camp", DateTime.Parse("2023-02-14"));
        List<StudentWork> studentWorks = new List<StudentWork>();
        studentWorks.Add(firststudent);
        studentWorks.Add(secondStudent);


        Assert.AreEqual(students[0].Name, firststudent.Name);
        Assert.AreEqual(students[0].NameOfWork, firststudent.NameOfWork);
        Assert.AreEqual(students[0].DateOfWork, firststudent.DateOfWork);

        Assert.AreEqual(students[1].DateOfWork, secondStudent.DateOfWork);
        Assert.AreEqual(students[1].DateOfWork, secondStudent.DateOfWork);
        Assert.AreEqual(students[1].DateOfWork, secondStudent.DateOfWork);   
    }

    [TestMethod]
    public void StudentWorkParsingErrorStrings()
    {
        string[] rawStrings = { "\"Bob\" \"Some work\" 2012-12-12", "  heheh" , "  \"Jill\"      \"Forest camp\"             2023-02-14" , "    ", " asdasdasd"};
        List<StudentWork> students = Lab3.ParseFile.FileParser(rawStrings);
        List<StudentWork> trueStudents = new List<StudentWork>();

        trueStudents.Add(new StudentWork("Bob", "Some work", DateTime.Parse("2012-12-12")));
        trueStudents.Add(new StudentWork("Jill", "Forest camp", DateTime.Parse("2023-02-14")));

        for (int i = 0; i < students.Count; i++)
        {
            Assert.AreEqual(trueStudents[i].Name, students[i].Name);
            Assert.AreEqual(trueStudents[i].NameOfWork, students[i].NameOfWork);
            Assert.AreEqual(trueStudents[i].DateOfWork, students[i].DateOfWork);
        }
    }

    [TestMethod]
    public void LoadObjectsToEmptyList()
    {
        bool isLoad = false;
        ObservableCollection<StudentWork> studentWorks = new();
        ObservableCollection<StudentWork> trueStudentWorks = new();
        trueStudentWorks.Add(new StudentWork("Bob", "Some work", DateTime.Parse("2012-12-12")));
        trueStudentWorks.Add(new StudentWork("Jill", "Forest camp", DateTime.Parse("2023-02-14")));
        string[] rawStrings = { "\"Bob\" \"Some work\" 2012-12-12", "  \"Jill\"      \"Forest camp\"             2023-02-14" };
        List<Lab3.StudentWork> students = Lab3.ParseFile.FileParser(rawStrings);
        ParseFile.LoadObjects(students, ref studentWorks, ref isLoad);

        for (int i = 0; i < students.Count; i++)
        {
            Assert.AreEqual(trueStudentWorks[i].Name, studentWorks[i].Name);
            Assert.AreEqual(trueStudentWorks[i].NameOfWork, studentWorks[i].NameOfWork);
            Assert.AreEqual(trueStudentWorks[i].DateOfWork, studentWorks[i].DateOfWork);
            Assert.AreEqual(true, isLoad);
        }
    }
    [TestMethod]
    public void LoadObjectsToNotEmptyList()
    {
        bool isLoad = false;
        ObservableCollection<StudentWork> studentWorks = new();
        ObservableCollection<StudentWork> trueStudentWorks = new();
        trueStudentWorks.Add(new StudentWork("Bob", "Some work", DateTime.Parse("2012-12-12")));
        trueStudentWorks.Add(new StudentWork("Jill", "Forest camp", DateTime.Parse("2023-02-14")));
        studentWorks.Add(new StudentWork("Jill", "Forest camp", DateTime.Parse("2023-02-14")));
        string[] rawStrings = { "\"Bob\" \"Some work\" 2012-12-12" };
        List<Lab3.StudentWork> students = Lab3.ParseFile.FileParser(rawStrings);
        ParseFile.LoadObjects(students, ref studentWorks, ref isLoad);

        for (int i = 0; i < students.Count; i++)
        {
            Assert.AreEqual(trueStudentWorks[i].Name, studentWorks[i].Name);
            Assert.AreEqual(trueStudentWorks[i].NameOfWork, studentWorks[i].NameOfWork);
            Assert.AreEqual(trueStudentWorks[i].DateOfWork, studentWorks[i].DateOfWork);
            Assert.AreEqual(true, isLoad);
        }
    }
    [TestMethod]
    public void LoadObjectsToList()
    {
        bool isLoad = true;
        ObservableCollection<StudentWork> studentWorks = new();
        ObservableCollection<StudentWork> trueStudentWorks = new();
        trueStudentWorks.Add(new StudentWork("Jill", "Forest camp", DateTime.Parse("2023-02-14")));
        studentWorks.Add(new StudentWork("Bob", "Jump camp", DateTime.Parse("2021-02-14")));
        string[] rawStrings = { "  \"Jill\"      \"Forest camp\"             2023-02-14" };
        List<Lab3.StudentWork> students = Lab3.ParseFile.FileParser(rawStrings);
        ParseFile.LoadObjects(students, ref studentWorks, ref isLoad);

        for (int i = 0; i < students.Count; i++)
        {
            Assert.AreEqual(trueStudentWorks[i].Name, studentWorks[i].Name);
            Assert.AreEqual(trueStudentWorks[i].NameOfWork, studentWorks[i].NameOfWork);
            Assert.AreEqual(trueStudentWorks[i].DateOfWork, studentWorks[i].DateOfWork);
            Assert.AreEqual(true, isLoad);
        }
    }


    [TestMethod]
    public void FilterTest1()
    {
        ObservableCollection<StudentWork> trueStudentWorks = new();
        trueStudentWorks.Add(new StudentWork("Bob", "Some work", DateTime.Parse("2012-12-12")));
        trueStudentWorks.Add(new StudentWork("Jill", "Forest camp", DateTime.Parse("2023-02-14")));

        ObservableCollection<StudentWork> studentWorks = new();

        studentWorks.Add(new StudentWork("Bob", "Some work", DateTime.Parse("2012-12-12")));
        studentWorks.Add(new StudentWork("Jill", "Forest camp", DateTime.Parse("2023-02-14")));

        ObservableCollection<StudentWork> studentWorks1 = ParseFile.Filter(studentWorks, "");

        for (int i = 0; i < studentWorks1.Count; i++)
        {
            Assert.AreEqual(trueStudentWorks[i].Name, studentWorks1[i].Name);
            Assert.AreEqual(trueStudentWorks[i].NameOfWork, studentWorks1[i].NameOfWork);
            Assert.AreEqual(trueStudentWorks[i].DateOfWork, studentWorks1[i].DateOfWork);
        }

    }

    [TestMethod]
    public void FilterTest2()
    {
        ObservableCollection<StudentWork> trueStudentWorks = new();
        trueStudentWorks.Add(new StudentWork("Bob", "Some work", DateTime.Parse("2012-12-12")));

        ObservableCollection<StudentWork> studentWorks = new();

        studentWorks.Add(new StudentWork("Bob", "Some work", DateTime.Parse("2012-12-12")));
        studentWorks.Add(new StudentWork("Jill", "Forest camp", DateTime.Parse("2023-02-14")));

        ObservableCollection<StudentWork> studentWorks1 = ParseFile.Filter(studentWorks, "Bob");

        for (int i = 0; i < studentWorks1.Count; i++)
        {
            Assert.AreEqual(trueStudentWorks[i].Name, studentWorks1[i].Name);
            Assert.AreEqual(trueStudentWorks[i].NameOfWork, studentWorks1[i].NameOfWork);
            Assert.AreEqual(trueStudentWorks[i].DateOfWork, studentWorks1[i].DateOfWork);
        }
    }

    [TestMethod]
    public void FilterNoMatchesReturnsEmptyCollection()
    {
        ObservableCollection<StudentWork> studentWorks = new();
        studentWorks.Add(new StudentWork("Bob", "Some work", DateTime.Parse("2012-12-12")));
        studentWorks.Add(new StudentWork("Jill", "Forest camp", DateTime.Parse("2023-02-14")));

        var filtered = ParseFile.Filter(studentWorks, "Alice");
        Assert.IsNotNull(filtered);
        Assert.AreEqual(0, filtered.Count);
    }
}
