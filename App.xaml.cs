using System.Configuration;
using System.Data;
using System.Windows;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Dictionary<int, SortedDictionary<int, List<List<string>>>> Tasks = new();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RemindTasks();
        }

        /// <summary>
        /// Adds a task with specified date, time, and details which contain name and description of the task.
        /// </summary>
        public static int AddTask(int date, int time, List<string> taskDetails) {
            try
            {
                if (Tasks.ContainsKey(date))
                {
                    if (Tasks[date].ContainsKey(time))
                    {
                        Tasks[date][time].Add(taskDetails);
                    }
                    else
                    {
                        Tasks[date].Add(time, [taskDetails]);
                    }
                }
                else
                {
                    Tasks.Add(date, new SortedDictionary<int, List<List<string>>>() { { time, [taskDetails] } });
                }
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// Modifies an already added task.
        /// </summary>
        public static int ModifyTask(int date, int time, int taskId, List<string> task)
        {
            try
            {
                if (Tasks.ContainsKey(date))
                {
                    if (Tasks[date].ContainsKey(time))
                    {
                        Tasks[date][time][taskId] = task;
                        return 0;
                    }
                }
                return 2;
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// Deletes a task (or "marks" it as done).
        /// </summary>
        public static int RemoveTask(int date, int time, int taskId)
        {
            try
            {
                if (Tasks.ContainsKey(date))
                {
                    if (Tasks[date].ContainsKey(time))
                    {
                        Tasks[date][time].RemoveAt(taskId);
                        if (Tasks[date][time].Count == 0) {
                            Tasks[date].Remove(time);
                        }
                        if (Tasks[date].Count == 0) {
                            Tasks.Remove(date);
                        }
                        return 0;
                    }
                }
                return 2;
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// Adds a task with specified date, time, and details which contain name and description of the task.
        /// </summary>
        public static SortedDictionary<int, List<List<string>>> GetDayTasks(int date)
        {
            try
            {
                return Tasks[date];
            }
            catch
            {
                return new SortedDictionary<int, List<List<string>>>();
            }
        }

        /// <summary>
        /// Reminds the user of past tasks (minimum same time as current time).
        /// Requires an implementation of a toaster or notification method which is not currently implemented.
        /// The task is deleted after being notified.
        /// </summary>
        private async void RemindTasks()
        {
            while (true)
            {
                int currentDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                if (Tasks.ContainsKey(currentDate))
                {
                    int currentTime = int.Parse(DateTime.Now.ToString("HHmm"));
                    if (Tasks[currentDate].Keys.Min() <= currentTime)
                    {
                        //new ToastContentBuilder()
                        //    .AddText($"Task Reminder: {Tasks[currentDate][currentTime][0][0]}")
                        //    .AddText($"{Tasks[currentDate][currentTime][0][1]}")
                        //    .Show();
                        RemoveTask(currentDate, currentTime, 0);
                    }
                }
                await Task.Delay(5000);
            }
        }
    }
}
