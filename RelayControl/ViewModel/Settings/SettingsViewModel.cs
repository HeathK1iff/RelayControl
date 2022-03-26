using MqttDevices.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MqttDevices.Utils;
using MqttDevices.ViewModel.Settings.DefenitionEditor.Commands;
using MqttDevices.Model.Configuration;

namespace MqttDevices.ViewModel
{
    public class SettingsViewModel : ModelViewBase
    {
        private readonly Config _settings;
        private DeviceDefenition _selectedItem;
        private ObservableCollectionEx<DeviceDefenition> _deviceDefenitions;
        public SettingsViewModel() { }
        public SettingsViewModel(Config settings)
        {
            _settings = settings;
            SaveSettingsCommand = new GenericCommand();

            _deviceDefenitions = new ObservableCollectionEx<DeviceDefenition>(_settings.DeviceDefenitions);
            
            EditDefenitionCommand = new EditDefenitionCommand(_deviceDefenitions);
            RemoveDefenitionCommand = new GenericCommand((item) =>
            {
               if (item is DeviceDefenition selectedItem)
               {
                    _deviceDefenitions.Remove(selectedItem);
               }
            }, null);
        }

        public string Host
        {
            get => _settings.Host;
            set
            {
                _settings.Host = value;
                DoPropertyChanged();
            }
        }
        public DeviceDefenition SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                DoPropertyChanged();
            }
        }
        public ObservableCollection<DeviceDefenition> DeviceDefenition => _deviceDefenitions;

        public ICommand EditDefenitionCommand { get; set; }
        public ICommand RemoveDefenitionCommand { get; set; }
        public ICommand SaveSettingsCommand { get; set; }
    }
}
