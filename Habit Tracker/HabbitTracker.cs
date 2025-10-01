namespace HabitTrackerApp;
class HabitTracker
{
    internal bool IsRunning { get; set; }
    internal Dictionary<string, Habit> Habits;

    internal List<Command> CommandHistory;

    public HabitTracker()
    {
        Habits = [];
        CommandHistory = [];
        Run();
    }

    private void GetCommand()
    {
        Console.WriteLine("Enter command. Type \"HELP\" for list of commands.");
        string? input = Console.ReadLine();
        if (input != null)
        {
            Command c = new(input.Split(' '), this);
            c.Execute();
            CommandHistory.Add(c);
        }
        
    }

    private void Run()
    {
        IsRunning = true;
        Console.WriteLine("Hello welcome to habit tracker!");

        while (IsRunning) GetCommand();

        Console.WriteLine("Goodbye have a nice day!");

    }

    
    public static void Main(string[] args)
    {
        HabitTracker Tracker = new();
        Tracker.Run();

    }
}
