
using System.ComponentModel.Design;

namespace HabitTrackerApp;

class HabitTracker
{
    internal bool IsRunning { get; set; }
    internal Dictionary<string, Habit> TrackedHabits = new Dictionary<string, Habit>();

    

    private void GetCommand()
    {
        Console.WriteLine("Enter command. Type \"HELP\" for list of commands.");
        string[] input = Console.ReadLine().Split();
        Command c = new(input, this);
        c.Execute();
    }

    private void Run()
    {
        IsRunning = true;
        Console.WriteLine("Hello welcome to habit tracker!");
        string[] input;

        while (IsRunning) GetCommand();

        Console.WriteLine("Goodbye have a nice day!");

    }

    
    public static void Main(string[] args)
    {
        HabitTracker Tracker = new();
        Tracker.Run();

    }
}
