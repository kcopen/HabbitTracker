using System.Data;
using System.Formats.Asn1;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace HabitTrackerApp;

class Command
{
    public static readonly string[] NoParamCommands = ["QUIT", "HELP", "SHOW"];
    public static readonly string[] SingleParamCommands = ["COMPLETE, REMOVE", "HELP", "SHOW"];
    public static readonly string[] DoubleParamCommands = ["ADD"];
    private string Type { get; }
    private string[] Input { get; }

    private HabitTracker Tracker { get; }



    public Command(string[] InputCommand, HabitTracker Tracker)
    {
        this.Tracker = Tracker;
        this.Input = InputCommand;
        if (Input.Length == 0) Type = "";
        else Type = Input[0].ToUpper();
    }

    private bool IsValid()
    {

        int len = Input.Length; //number of params in the command
        if (len == 0) return false;
        else if (
            (len == 1 && NoParamCommands.Contains(Type)) ||
            (len == 2 && SingleParamCommands.Contains(Type)) ||
            (len == 3 && DoubleParamCommands.Contains(Type))
            ) return true;
        else return false;


    }

    public bool Execute()
    {
        if (!IsValid())
        {
            Write(string.Join(' ', Input) + ": is an invalid input format. Type \"HELP\" for valid formats.");
            return false;
        }
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
            case "SHOW":
                Show();
                break;
        }

        return true;

    }

    //helper method to shorten Console.Writeline()
    private static void Write(string s) { Console.WriteLine(s); }

    private void Help()
    {

        if (Input.Length < 2)
        {
            Write("Commands are format sensitive.");
            Write("For details about a particular command use the command name for param:");
            Write("HELP param");
            Write("Here is the list of valid commands.");

            foreach (string c in Command.NoParamCommands) Write(c);
            foreach (string c in Command.SingleParamCommands) Write($"{c} param1");
            foreach (string c in Command.DoubleParamCommands) Write($"{c} param1 param2");
            return;
        }
        string command = Input[1].ToUpper();
        switch (command)
        {
            case "Quit":
                Write($"{command} will quit the application.");
                break;
            case "HELP":
                Write($"{command} will bring up the help menu.");
                break;
            case "ADD":
                Write($"{command} (param1) (param2) will add a new habit to track.");
                Write("param1 is the name you wish to call the habit");
                Write("param2 is the description of the habit");
                Write("Parameters must be placed in parenthesis.");
                break;
            case "REMOVE":
                Write($"{command} (param1) will remove the named habit to track.");
                Write("param1 is the name of the habit to remove.");
                Write("Parameters must be placed in parenthesis.");
                break;
            case "COMPLETE":
                Write($"{command} (param1) will add one count to the named habit. Use when you complete a habit.");
                Write("param1 is the name of the habit to complete.");
                Write("Parameters must be placed in parenthesis.");
                break;
            case "SHOW":
                Write($"{command} will show your progress on all your tracked habits.");
                Write("SHOW (param1) will show your progress on a particular habit.");
                Write("param1 is the name of the habit to show.");
                Write("Parameters must be placed in parenthesis.");
                break;
        }

    }

    private void AddNewHabbit(string HabitName, string HabitDescription)
    {
        Tracker.Habits.Add(HabitName, new Habit(HabitName, HabitDescription));
    }


    private void Quit()
    {
        Tracker.IsRunning = false;
    }

    private void CompleteHabit(string Name)
    {
        Habit habitToComplete = Tracker.Habits[Name];
        if (habitToComplete == null)
        {
            Write($"The habit {Input[1]} was not found.");
        }
        else habitToComplete.Complete();
    }

    private void RemoveHabit(string Name)
    {
        Tracker.Habits.Remove(Name);
    }

    private void Show()
    {
        if (Input.Length == 2)
        {
            Habit habitToShow = Tracker.Habits[Input[1]];
            if (habitToShow == null)
            {
                Write($"The habit {Input[1]} was not found.");
                return;
            }

            foreach (DateTime d in habitToShow.TimeStamps)
            {
                Write(d.ToString());
            }
        }
        else
        {
            foreach (KeyValuePair<string, Habit> h in Tracker.Habits)
            {
                foreach (DateTime d in h.Value.TimeStamps)
                {
                    Write(d.ToString());
                }
            }
        }

    }

    public static string[] Parse(string Input)
    {
        List<string> commandList = [];
        if (Input.Length < 1) return [];

        int i = Input.IndexOf(' ');
        commandList[0] = Input[..i].Trim().ToUpper();
        Input = Input[i..].Trim();

        string NextParam()
        {
            string param = "";
            if (Input.StartsWith('(') && Input.Contains(')'))
            {
                i = Input.IndexOf(')');
                param = Input[1..(i - 1)];
                Input = Input.Length >= i + 1 ? Input[(i + 1)..].Trim() : "";
            }
            return param;
        }

        while (Input.Length > 0) commandList.Add(NextParam());
        return [.. commandList];
    }
   
}