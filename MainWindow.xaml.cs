using System.Windows;

namespace TempMailBox
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            if (DataContext is ViewModels.MainViewModel vm)
            {
                vm.Cleanup();
            }
        }
    }
}
