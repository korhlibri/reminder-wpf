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
    /// Interaction logic for TaskDialog.xaml
    /// </summary>
    public partial class TaskDialog : Window
    {
        public string currentOperation = "ADD";
        public int dateOp = 0;
        public int timeOp = 0;
        public int taskOp = -1;

        public TaskDialog(string operation, int taskId = 0, int date = 0, int time = 0)
        {
            InitializeComponent();

            currentOperation = operation;

            if (operation == "MOD")
            {
                taskDate.IsEnabled = false;
                taskTime.IsEnabled = false;
                dateOp = date;
                timeOp = time;
                taskOp = taskId;
            }
            else
            {
                taskDate.IsEnabled = true;
                taskTime.IsEnabled = true;
            }
        }

        private void SubmitNewTask(object sender, RoutedEventArgs e)
        {
            if (currentOperation == "MOD")
            {
                string name = taskName.Text;
                string desc = taskDesc.Text;
                List<string> data = [name, desc];

                App.ModifyTask(dateOp, timeOp, taskOp, data);
                MainWindow.UpdateDayTasks();

                DialogResult = true;
            }
            else
            {
                string name = taskName.Text;
                string desc = taskDesc.Text;
                string date = taskDate.Text;
                string time = taskTime.Text;
                int parseddate = int.Parse(String.Join("", date.Split("-")));
                int parsedtime = int.Parse(String.Join("", time.Split(":")));
                List<string> data = [name, desc];

                App.AddTask(parseddate, parsedtime, data);
                MainWindow.UpdateDayTasks();

                DialogResult = true;
            }
        }
    }
}