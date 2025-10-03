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
            string[] cArray =  Command.Parse(input);
            if (cArray.Length < 1) return;
            
            Command commandToExecute = new(cArray, this);
            commandToExecute.Execute();
            CommandHistory.Add(commandToExecute);
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
