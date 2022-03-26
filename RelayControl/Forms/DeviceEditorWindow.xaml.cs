using System.Windows;

namespace MqttDevices.Forms
{
    /// <summary>
    /// Interaction logic for DefenitionDetailsWindow.xaml
    /// </summary>
    public partial class DeviceEditorWindow : Window
    {
        public DeviceEditorWindow() { 
            InitializeComponent();
        }

        private void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
