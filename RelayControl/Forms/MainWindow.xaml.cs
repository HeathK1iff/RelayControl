using MqttDevices.ViewModel.Main;
using System.Windows;
using System.Windows.Input;

namespace MqttDevices.Forms
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
        }

        private void CloseForm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
