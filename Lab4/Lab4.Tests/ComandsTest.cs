namespace Lab4.Tests;

[TestClass]
public class ComandsTest
{
    [TestMethod]
    public void AddCommand_AddsStudentWork()
    {
        var added = new List<StudentWork>();
        var parser = new ParseCommands
        {
            AddWorkAction = sw => added.Add(sw),
            RemoveWorksAction = _ => { },
            GetWorksFunc = () => added,
            SaveWorks = _ => { }
        };
        string[] lines = { "ADD John;Math;2023-01-01" };
        parser.ProcessCommandFile(lines);

        Assert.AreEqual("John", added[0].Name);
        Assert.AreEqual("Math", added[0].NameOfWork);
        Assert.AreEqual(new DateTime(2023, 1, 1), added[0].DateOfWork);
    }

    [TestMethod]
    public void RemoveCommand_RemovesByName()
    {
        var works = new List<StudentWork>
        {
            new StudentWork("John", "Math", new DateTime(2023, 1, 1)),
            new StudentWork("Jane", "Physics", new DateTime(2023, 2, 2))
        };
        List<StudentWork> removed = null;
        var parser = new ParseCommands
        {
            AddWorkAction = _ => { },
            RemoveWorksAction = r => removed = r.ToList(),
            GetWorksFunc = () => works,
            SaveWorks = _ => { }
        };
        string[] lines = { "REMOVE Name=John" };
        parser.ProcessCommandFile(lines);
        Assert.IsNotNull(removed);
        Assert.AreEqual(1, removed.Count);
        Assert.AreEqual("John", removed.First().Name);
    }
    [TestMethod]
    public void SaveCommand_CallsSaveWorks()
    {
        var works = new List<StudentWork>
        {
            new StudentWork("John", "Math", new DateTime(2023, 1, 1)),
            new StudentWork("Jane", "Physics", new DateTime(2023, 2, 2))
        };
        string savedPath = null;
        var parser = new ParseCommands
        {
            AddWorkAction = _ => { },
            RemoveWorksAction = _ => { },
            GetWorksFunc = () => works,
            SaveWorks = path => savedPath = path
        };
        string[] lines = { "SAVE test.txt" };
        parser.ProcessCommandFile(lines);
        Assert.AreEqual("test.txt", savedPath);
    }
    [TestMethod]
    public void UnknownCommand_LogsError()
    {
        bool errorLogged = false;
        Logger.OnLog = msg => { if (msg.Contains("Неизвестная команда")) errorLogged = true; };
        var parser = new ParseCommands
        {
            AddWorkAction = _ => { },
            RemoveWorksAction = _ => { },
            GetWorksFunc = () => Enumerable.Empty<StudentWork>(),
            SaveWorks = _ => { }
        };
        string[] lines = { "UNKNOWN something" };
        parser.ProcessCommandFile(lines);
        Assert.IsTrue(errorLogged);
    }
}
