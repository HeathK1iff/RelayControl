using MqttDevices.Forms;
using MqttDevices.Utils;
using MqttDevices.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace MqttDevices.Commands
{
    class OpenSettingCommand : GenericCommand
    {
        public OpenSettingCommand() {}

        public OpenSettingCommand(IModelView modelView, Action<object> execute, Func<object, bool> canExecute) : base(modelView, execute, canExecute) {}

        protected override void DoExecute(object parameter)
        {
            OptionWindow form = new();
            form.Owner = Application.Current.MainWindow;

            if (Application.Current is App appl)
            {
                var viewModel = new SettingsViewModel(appl.Settings);

                if (viewModel is SettingsViewModel dataContext)
                {
                    if (dataContext.SaveSettingsCommand is ICommandEvents events)
                    {
                        events.OnSuccess += () =>
                        {
                            appl.Settings.DeviceDefenitions.Clear();
                            appl.Settings.DeviceDefenitions.AddRange(dataContext.DeviceDefenition);

                            try
                            {
                                appl.Settings.SaveAsync(appl.ConfigFileName);
                                form.Dispatcher.Invoke(() =>
                                {

                                    appl.RefreshDevices();
                                    ModelView.Refresh();
                                    form.Close();
                                });
                            } 
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                                form.Close();
                            }
                        };

                        events.OnError += (e) =>
                        {
                            form.Dispatcher.Invoke(() =>
                            {
                                MessageBox.Show(e.Message);
                                form.Close();
                            });
                        };
                    }
                }
                form.DataContext = viewModel;
            }
            
            form.ShowDialog();
        }
    }
}
