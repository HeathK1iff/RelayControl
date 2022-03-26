using MqttDevices.Forms;
using MqttDevices.Model;
using MqttDevices.Model.Configuration;
using MqttDevices.Utils;
using System.Windows;

namespace MqttDevices.ViewModel.Settings.DefenitionEditor.Commands
{
    class EditDefenitionCommand : GenericCommand
    {
        private ObservableCollectionEx<DeviceDefenition> _defenitions;
        public EditDefenitionCommand(ObservableCollectionEx<DeviceDefenition> defenitions)
        {
            _defenitions = defenitions;
        }

        protected override void DoExecute(object parameter)
        {
            var form = new DeviceEditorWindow();


            if (Application.Current is App win)
            {
                form.Owner = win.GetActiveWindow();
            }


            DeviceDefenition device = null;
            if (parameter is DeviceDefenition dev)
                device = dev;

            if (device == null)
                device = new DeviceDefenition();

            var dataContext = new DefenitionEditorViewModel(device);

            if (dataContext.CloseCommand is ICommandEvents closeEvents)
            {
                closeEvents.OnSuccess += () =>
                {
                    form.Close();
                };
            }

            if (dataContext.SaveCommand is ICommandEvents events)
            {
                events.OnSuccess += () =>
                {
                    if (parameter == null)
                    {
                        _defenitions.Add(device);
                    }
                    _defenitions.NotifyItemChanged();
                    

                    if (Application.Current is App appl)
                    {
                        appl.RefreshDevicesAsync();
                    }

                    form.Close();

                };

                events.OnError += (e) =>
                {
                    MessageBox.Show(e.Message);
                    form.Close();
                };
            }
            form.DataContext = dataContext;

            form.ShowDialog();
        }
    }
}
