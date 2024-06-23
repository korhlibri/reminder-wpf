using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public MainWindow()
        {
            InitializeComponent();

            //DisplayableTasks.Add("2024-06-19 00:00 Test");

            taskList.ItemsSource = DisplayableTasks;
        }

        public static void UpdateDayTasks()
        {
            DayTasks = App.GetDayTasks(DateSelected);
            SetDisplayableTasks();
        }

        public static void SetDisplayableTasks()
        {
            DisplayableTasks = [];
            TaskDetailBinding = [];
            foreach (KeyValuePair<int, List<List<string>>> timeTasks in DayTasks)
            {
                foreach (List<string> task in timeTasks.Value)
                {
                    DisplayableTasks.Add($"{DateSelected / 10000}-{DateSelected / 10000 - DateSelected % 100}-{DateSelected % 100} {timeTasks.Key / 100}:{timeTasks.Key % 100}   {task[0]}");
                    TaskDetailBinding.Add(task);
                }
            }
        }

        private void AddTaskAction(object sender, RoutedEventArgs e)
        {
            var newTaskDialog = new TaskDialog("ADD");

            newTaskDialog.ShowDialog();
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