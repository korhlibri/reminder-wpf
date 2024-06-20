using System.Collections.ObjectModel;
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
        private static int DateSelected = int.Parse(DateTime.Now.ToString("yyyyMd"));
        private static SortedDictionary<int, List<List<string>>> DayTasks = new();
        private static ObservableCollection<string> DisplayableTasks = new();

        public MainWindow()
        {
            InitializeComponent();

            DisplayableTasks.Add("2024-06-19 00:00 Test");

            taskList.ItemsSource = DisplayableTasks;
        }

        private void UpdateDayTasks()
        {
            DayTasks = App.GetDayTasks(DateSelected);
            SetDisplayableTasks();
        }

        private void SetDisplayableTasks()
        {
            ObservableCollection<string> ReadableTasks = [];
            foreach (KeyValuePair<int, List<List<string>>> timeTasks in DayTasks)
            {
                foreach (List<string> task in timeTasks.Value)
                {
                    ReadableTasks.Add($"{DateSelected / 10000}-{DateSelected / 10000 - DateSelected % 100}-{DateSelected % 100}  {timeTasks.Key / 100}:{timeTasks.Key % 100} {task[0]}");
                }
            }
            DisplayableTasks = ReadableTasks;
        }

        private void AddTaskAction(object sender, RoutedEventArgs e)
        {
            var newTaskDialog = new TaskDialog("ADD");

            newTaskDialog.ShowDialog();
        }
    }
}