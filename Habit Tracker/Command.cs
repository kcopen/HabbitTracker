using System.Data;
using System.Net.NetworkInformation;

namespace HabitTrackerApp;

class Command
{
    public static readonly string[] SingleParamCommands = ["QUIT", "HELP", "SHOW"];
    public static readonly string[] DoubleParamCommands = ["COMPLETE, REMOVE", "HELP", "SHOW"];
    public static readonly string[] TripleParamCommands = ["ADD"];
    private string Type { get; }
    private string[] Input { get; }

    private HabitTracker Tracker { get; }



    public Command(string[] Input, HabitTracker Tracker)
    {
        this.Tracker = Tracker;
        this.Input = Input;
        if (Input.Length > 0) Type = Input[0].ToUpper();
    }

    private static bool IsValid(string[] Command)
    {
        int len = Command[0].Length; //number of params in the command
        if (
            (len == 1 && SingleParamCommands.Contains(Command[0])) ||
            (len == 2 && DoubleParamCommands.Contains(Command[0])) ||
            (len == 3 && TripleParamCommands.Contains(Command[0]))
            ) return true;

        Console.WriteLine("Invalid input format. Type \"HELP\" for valid formats.");
        return false;
    }

    public bool Execute()
    {
        if (!IsValid(Input)) return false;
        switch (Type)
        {
            case "QUIT":
                Quit();
                break;
            case "HELP":
                Help();
                break;
            case "ADD":
                AddNewHabbit(Input[1], Input[2]);
                break;
            case "COMPLETE":
                CompleteHabit(Input[1]);
                break;
            case "REMOVE":
                RemoveHabit(Input[1]);
                break;
            default:
                Help(Type);
                break;
        }

        return true;

    }

    //helper method to shorten Console.Writeline()
    private void w(string s) { Console.WriteLine(s); }

    private void Help(string command)
    {
        switch (command)
        {
            case "Quit":
                w($"{command} will quit the application.");
                break;
            case "HELP":
                w($"{command} will bring up the help menu.");
                break;
            case "ADD":
                w($"{command} param1 param2 will add a new habit to track.");
                w("param1 is the name you wish to call the habit");
                w("param2 is the description of the habit");
                break;
            case "REMOVE":
                w($"{command} param1 will remove the named habit to track.");
                w("param1 is the name of the habit to remove.");
                break;
            case "COMPLETE":
                w($"{command} param1 will add one count to the named habit. Use when you complete a habit.");
                w("param1 is the name of the habit to complete.");
                break;
            case "SHOW":
                w($"{command} will show your progress on all your tracked habits.");
                w("SHOW param1 will show your progress on a particular habit.");
                w("param1 is the name of the habit to show.");
                break;
            default:
                Help();
                break;
        }
        
    }
    private void Help()
    {
        w("Commands are format sensitive.");
        w("For details about a particular command use the command name for param:");
        w("HELP param");
        w("Here is the list of valid commands.");

        foreach (string c in Command.SingleParamCommands) w(c);
        foreach (string c in Command.SingleParamCommands) w($"{c} param1");
        foreach (string c in Command.SingleParamCommands) w($"{c} param1 param2");

    }

    private void AddNewHabbit(string HabitName, string HabitDescription)
    {
        Tracker.TrackedHabits.Add(HabitName, new Habit(HabitName, HabitDescription));
    }


    private void Quit()
    {
        Tracker.IsRunning = false;
    }

    private void CompleteHabit(string Name)
    {
        Tracker.TrackedHabits[Name].Complete();
    }

    private void RemoveHabit(string Name)
    {
        Tracker.TrackedHabits.Remove(Name);
    }

   
}