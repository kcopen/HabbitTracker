
using System.Globalization;

namespace HabitTrackerApp;

class Habit
{
    internal string Name { get; }
    internal string Description { get; }
    internal int DailyStreak { get; set; } //how many days in a row the habit has been completed

    internal List<DateTime> TimeStamps { get; set; }

    private readonly string FilePath;

    public Habit(string Name, string Description)
    {
        this.Name = Name;
        this.Description = Description;
        this.FilePath = Directory.GetCurrentDirectory() + "\\..\\..\\..\\habits\\" + Name + ".txt";
        this.TimeStamps = [];
        LoadData();

    }

    public void Complete()
    {
        TimeStamps.Add(DateTime.Now);
        UpdateStreak();
        SaveData();
    }

    private void UpdateStreak()
    {
        DailyStreak = 0;
        if (TimeStamps.Count == 0) return;
        DateTime MostRecent = TimeStamps[^1];

        //if most recent completion was not today or yesterday then DailyStreak is 0 otherwise increment streak by 1
        TimeSpan interval = MostRecent - DateTime.Today;
        if (interval.Days > 1 && MostRecent.Day != DateTime.Today.Day) return;
        else DailyStreak++;

        //loop through the completions and increment DailyStreak every time there is an interval of 1
        //if the increment is greater than 1 then return
        for (int i = 2; i <= TimeStamps.Count - 1; i++)
        {
            interval = TimeStamps[^i] - TimeStamps[^(i + 1)];
            if (interval.Days == 1) DailyStreak++;
            else if (interval.Days > 1) return;
        }
    }

    public int TotalCompletions()
    {
        return TimeStamps.Count;
    }

    public bool SaveData()
    {
        try
        {
            using (StreamWriter sw = File.AppendText(FilePath))
            {
                sw.WriteLine(DateTime.Now.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    public bool LoadData()
    {
        try
        {
            if (!File.Exists(FilePath)) File.Create(FilePath);

            using StreamReader sr = new(FilePath);
            string? line;
            string format = "f";
            DateTime date;

            while ((line = sr.ReadLine()) != null)
            {
                date = DateTime.ParseExact(line, format, CultureInfo.CurrentCulture);
                TimeStamps.Add(date);
            }
            UpdateStreak();
        }
        catch (Exception e)
        {
            UpdateStreak();
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
}
