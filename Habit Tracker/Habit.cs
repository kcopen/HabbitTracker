using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
namespace HabitTrackerApp
{
    class Habit
    {
        public string Name { get; }
        public string Description { get; }
        public int CurrentDailyStreak { get; set; } //how many days in a row the habit has been completed

        private List<DateTime> CompletionDateTimes { get; set; }

        private readonly string FilePath;


        public Habit(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
            this.FilePath = Directory.GetCurrentDirectory() +  "\\..\\..\\..\\habits\\" + Name + ".txt";
            this.CompletionDateTimes = [];
            this.CurrentDailyStreak = 0;
            UpdateStreak(false);
            

            try
            {
                if (!File.Exists(FilePath))
                {
                    File.Create(FilePath);
                }

                using (StreamReader sr = new StreamReader(FilePath))
                {
                    string line;
                    DateTime date = new();

                    while ((line = sr.ReadLine()) != null)
                    {
                        DateTime.TryParse(line, out date);
                        CompletionDateTimes.Add(date);
                    }
                }
            
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public void Complete()
        {
            try
            {
                DateTime CurrentTime = DateTime.Now;
                using (StreamWriter sw = File.AppendText(FilePath))
                {
                    sw.WriteLine(CurrentTime.ToString());
                }

                CompletionDateTimes.Add(CurrentTime);
                UpdateStreak(true);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private void UpdateStreak(bool HabitCompletedToday)
        {
            DateTime Today = DateTime.Today;
            DateTime Yesterday = Today.AddDays(-1);

            //add one to habit streak if the habit was completed today
            if (!HabitCompletedToday)
            {
                //if the habit was not completed today or yesterday then reset the streak
                if
                (
                    CompletionDateTimes.Count > 0 &&
                    (CompletionDateTimes[CompletionDateTimes.Count - 1] != Today || CompletionDateTimes[CompletionDateTimes.Count - 1] != Yesterday)
                )
                {
                    CurrentDailyStreak = 0;
                }
                return;
            }
            //habit was completed today 
            if (CompletionDateTimes.Count == 1)
            {
                CurrentDailyStreak = 1;
            }
            else if (CompletionDateTimes.Count > 1)
            {
                //if the habit was completed yesterday then add one to the streak otherwise reset streak to 1
                if (CompletionDateTimes[CompletionDateTimes.Count - 2] == Yesterday)
                {
                    CurrentDailyStreak++;
                }
                else
                {
                    CurrentDailyStreak = 1;
                }

            }
        }

        public int TotalCompletions()
        {
            return CompletionDateTimes.Count;
        }
    }
}