using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MqttDevices.Utils
{
    public class ModelViewBase : INotifyPropertyChanged
    {
        public ModelViewBase() {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void DoPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
