
using System.Globalization;

namespace HabitTrackerApp;

class Habit
{
    public string Name { get; }
    public string Description { get; }
    public int DailyStreak { get; set; } //how many days in a row the habit has been completed

    private List<DateTime> Completions { get; set; }

    private readonly string FilePath;


    public Habit(string Name, string Description)
    {
        this.Name = Name;
        this.Description = Description;
        this.FilePath = Directory.GetCurrentDirectory() + "\\..\\..\\..\\habits\\" + Name + ".txt";
        this.Completions = [];
        LoadData();

    }

    public void Complete()
    {
        Completions.Add(DateTime.Now);
        UpdateStreak();
        SaveData();
    }

    private void UpdateStreak()
    {
        DailyStreak = 0;
        if (Completions.Count == 0) return;
        DateTime MostRecent = Completions[^1];

        //if most recent completion was not today or yesterday then DailyStreak is 0 otherwise increment streak by 1
        TimeSpan interval = MostRecent - DateTime.Today;
        if (interval.Days > 1 && MostRecent.Day != DateTime.Today.Day) return;
        else DailyStreak++;

        //loop through the completions and increment DailyStreak every time there is an interval of 1
        //if the increment is greater than 1 then return
        for (int i = 2; i <= Completions.Count - 1; i++)
        {
            interval = Completions[^i] - Completions[^(i + 1)];
            if (interval.Days == 1) DailyStreak++;
            else if (interval.Days > 1) return;
        }
    }

    public int TotalCompletions()
    {
        return Completions.Count;
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
                Completions.Add(date);
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
