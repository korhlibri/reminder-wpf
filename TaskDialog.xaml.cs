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

        public TaskDialog(string operation, UInt32 taskId = 0)
        {
            InitializeComponent();
        }

        private void SubmitNewTask(object sender, RoutedEventArgs e)
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