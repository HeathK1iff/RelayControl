using MqttDevices.Model.Client;

namespace MqttDevices.Model
{
    public interface IDevice
    {
        void Subscribe(ISubscriber subscriber);
        string Topic { get; }
    }

    public abstract class Device: IDevice
    {
        protected IMqttClientService _client;
        protected abstract void DoSubscribe(ISubscriber subscriber);
        void IDevice.Subscribe(ISubscriber subscriber)
        {
            DoSubscribe(subscriber);
        }

        public Device(IMqttClientService client)
        {
            _client = client;
        }        
        
        public string Topic { get; set; }
        public string Name { get; set; }
    }
}
