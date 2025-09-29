
namespace HabitTrackerApp
{
    class HabitTracker
    {
        public Dictionary<string, Habit> TrackedHabits = new Dictionary<string, Habit>();
        private void GetStarted()
        {
            Console.WriteLine("Hello welcome to habit tracker!");
            string input = "";
            AddNewHabit("blah", "blah");
            while (input != "quit")
            {
                input = Console.ReadLine();
            }
            
        }

        private void AddNewHabit(string HabitName, string HabitDescription)
        {
            TrackedHabits.Add(HabitName, new Habit(HabitName, HabitDescription));
        }
        public static void Main(string[] args)
        {
            HabitTracker Tracker = new HabitTracker();
            Tracker.GetStarted();

        }
    }
}