using MqttDevices.Model.Configuration;
using MqttDevices.Utils;
using System.Windows.Input;

namespace MqttDevices.ViewModel.Settings.DefenitionEditor
{
    public class DefenitionEditorViewModel : ModelViewBase
    {
        DeviceDefenition _item;
        public DefenitionEditorViewModel(DeviceDefenition item) : base()
        {
            _item = item;

            SaveCommand = new GenericCommand(null, (obj) =>
            {
                return (!string.IsNullOrEmpty(_item.Name)) && (!string.IsNullOrEmpty(_item.Topic));
            });
        }

        public string Name
        {
            get => _item.Name;
            set
            {
                _item.Name = value;
                DoPropertyChanged();
            }
        }

        public string Topic
        {
            get => _item.Topic;
            set
            {
                _item.Topic = value;
                DoPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; } = new GenericCommand();
    }
}
