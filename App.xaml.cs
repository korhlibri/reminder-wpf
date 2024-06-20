using System.Configuration;
using System.Data;
using System.Windows;
using System.Collections.Generic;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int DateSelected = int.Parse(DateTime.Now.ToString("yyyyMd"));
        private static Dictionary<int, SortedDictionary<int, List<List<string>>>> Tasks = new();

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
                        Tasks[date] = new SortedDictionary<int, List<List<string>>>()
                        {
                            { time, [taskDetails] }
                        };
                    }
                }
                //else
                //{
                //    Tasks = new Dictionary<int, SortedDictionary<int, List<List<string>>>>()
                //    {
                //        {
                //            date,
                //            new SortedDictionary<int, List<List<string>>>()
                //            {
                //                { time, [taskDetails] }
                //            }
                //        }
                //    };
                //}
                return 0;
            }
            catch
            {
                return 1;
            }
        }

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

        public static SortedDictionary<int, List<List<string>>> getDayTasks(int date)
        {
            return Tasks[date];
        }
    }
}
