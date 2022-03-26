using MqttDevices.Commands;
using MqttDevices.Model;
using MqttDevices.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MqttDevices.ViewModel.Main
{
    public sealed class MainViewModel : ModelViewBase, IModelView
    {
        private bool _completedLoading;
        private ObservableCollectionEx<Device> _device = new ObservableCollectionEx<Device>();
        public void Refresh()
        {
            _device.Clear();
            if (Application.Current is App appl)
            {
                try
                {
                    Task initTask = Task.Run(async () => { await appl.InitAsync(); });
                    initTask.Wait();

                    foreach (var device in appl.Devices)
                        _device.Add(device);
                    _device.NotifyItemChanged();

                    _completedLoading = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public MainViewModel() : base()
        {
            Refresh();

            OpenSettingCommand = new OpenSettingCommand(this, null, (p) =>
            {
                return _completedLoading;
            });

            SwitchRelayCommand = new GenericCommand((obj) =>
            {
                if (obj is RelayDevice dev)
                {
                    dev.Switch(!dev.State);
                }
            },p => _completedLoading);
        }

        public ObservableCollection<Device> Devices
        {
            get => _device;
        }

        public ICommand SwitchRelayCommand { get; set; }
        public ICommand OpenSettingCommand { get; set; }

    }
}
