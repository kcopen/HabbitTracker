using System;
using System.IO;
using System.Collections.Generic;
class Habit
{
    private string Name { get; }
    private string Description { get; }
    private int CurrentDailyStreak { get; } //how many days in a row the habit has been completed

    private List<DateTime> CompletionDateTimes;

    private string FilePath;


    public void Habit(string Name, string Description)
    {
        this.Name = Name;
        this.Description = Description;
        this.CurrentDailyStreak = 0;
        this.CompletionDateTimes = new List<DateTime>();
        this.FilePath = $@"c:\habits\{Name}.txt";
        try
        {
            File file = File.OpenRead(FilePath);
            foreach (string line in file.ReadLines(FilePath)) {
                CompletionDateTimes.add(DateTime.Parse(line));
            }
            
        }
        catch (IOException e)
        {

        }
    }

    public void Complete()
    {
        try
        {
            DateTime CurrentTime = DateTime.Now;
            File file = File.OpenWrite(FilePath);
            file.WriteLine(CurrentTime);
            CompletionDateTimes.Add(CurrentTime);

            DateTime yesterday = Datetime.Today.AddDays(-1);
            if (CompletionDateTimes.Length == 1)
            {
                CurrentDailyStreak++;
            }
            else if (CompletionDateTimes.Length > 1)
            {
                if (CompletionDateTimes[CompletionDateTimes.Length - 2] == yesterday)
                {
                    CurrentDailyStreak++;
                }

            }
                    
            
        } catch (IOException e) {
            Console.WriteError(e);
        }
        
    }

    public int TotalCompletions() {
        return CompletionDateTimes.Length;
    }
}