using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int DateSelected = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
        private static SortedDictionary<int, List<List<string>>> DayTasks = new();
        private static List<string> DisplayableTasks = new();
        private static List<List<string>> TaskDetailBinding = new();
        private static List<List<int>> TaskTimeBinding = new();

        public MainWindow()
        {
            InitializeComponent();

            //DisplayableTasks.Add("2024-06-19 00:00 Test");

            taskList.ItemsSource = DisplayableTasks;
        }

        /// <summary>
        /// Starts the task updating process on the Main Window.
        /// </summary>
        public static void UpdateDayTasks()
        {
            DayTasks = App.GetDayTasks(DateSelected);
            SetDisplayableTasks();
        }

        /// <summary>
        /// Sets the tasks to be displayed on the Main Window.
        /// DisplayableTasks contains the text that will be displayed on the Window List.
        /// TaskDetailBinding contains the name and description of the task in a list, allowing for quick access.
        /// TaskTimeBinding contains the date and time of the task as integers.
        /// </summary>
        public static void SetDisplayableTasks()
        {
            DisplayableTasks = [];
            TaskDetailBinding = [];
            TaskTimeBinding = [];
            foreach (KeyValuePair<int, List<List<string>>> timeTasks in DayTasks)
            {
                foreach (List<string> task in timeTasks.Value)
                {
                    DisplayableTasks.Add($"{DateSelected / 10000}-{DateSelected / 10000 - DateSelected % 100}-{DateSelected % 100} {timeTasks.Key / 100}:{timeTasks.Key % 100}   {task[0]}");
                    TaskDetailBinding.Add(task);
                    TaskTimeBinding.Add([DateSelected, timeTasks.Key]);
                }
            }
        }

        /// <summary>
        /// Gets the real index of a task.
        /// Real index is an index of the task in the dictionary rather than on the displayable list.
        /// As the index is first gotten from the Window, it needs to be converted to a dictionary.
        /// All items have their own list based on the time of the task.
        /// This allows for multiple tasks to have the same time.
        /// However, this also means that indexes differ between dictionaries and displayable list on the Window.
        /// </summary>
        private static int GetRealIndex(int index)
        {
            int time = TaskTimeBinding[index][1];
            foreach (List<int> timeBinding in TaskTimeBinding)
            {
                if (timeBinding[1] != time)
                {
                    index -= 1;
                }
                else
                {
                    return index;
                }
            }
            return -1;
        }

        private void AddTaskAction(object sender, RoutedEventArgs e)
        {
            var newTaskDialog = new TaskDialog("ADD");

            newTaskDialog.ShowDialog();

            taskList.ItemsSource = DisplayableTasks;
            taskList.SelectedIndex = -1;
        }

        private void ModifyTaskAction(object sender, RoutedEventArgs e)
        {
            int index = taskList.SelectedIndex;
            if (index != -1)
            {
                int realindex = GetRealIndex(index);
                if (realindex != -1)
                {
                    var newTaskDialog = new TaskDialog("MOD", taskId: realindex, date: TaskTimeBinding[index][0], time: TaskTimeBinding[index][1]);

                    newTaskDialog.ShowDialog();

                    taskList.ItemsSource = DisplayableTasks;
                    taskList.SelectedIndex = -1;
                }
            }
        }

        private void DeleteTaskAction(object sender, RoutedEventArgs e)
        {
            int index = taskList.SelectedIndex;
            if (index != -1)
            {
                int realindex = GetRealIndex(index);
                if (realindex != -1)
                {
                    App.RemoveTask(TaskTimeBinding[index][0], TaskTimeBinding[index][1], realindex);

                    UpdateDayTasks();
                    taskList.ItemsSource = DisplayableTasks;
                    taskList.SelectedIndex = -1;
                }
            }
        }

        private void DateChangeAction(object sender, SelectionChangedEventArgs e)
        {
            DateSelected = int.Parse(taskDatePicker.SelectedDate.Value.ToString("yyyyMMdd"));

            UpdateDayTasks();
            taskList.ItemsSource = DisplayableTasks;
            taskList.SelectedIndex = -1;
        }

        private void ListBoxSelectAction(object sender, SelectionChangedEventArgs e)
        {
            int index = taskList.SelectedIndex;
            if (index != -1)
            {
                taskSelectedName.Text = TaskDetailBinding[index][0];
                taskSelectedDesc.Text = TaskDetailBinding[index][1];
            }
            else
            {
                taskSelectedName.Text = "None";
                taskSelectedDesc.Text = "None";
            }
        }
    }
}