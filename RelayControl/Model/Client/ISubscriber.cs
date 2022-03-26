using System;

namespace MqttDevices.Model.Client
{
    public interface ISubscriber
    {
        void Subscribe(string mqttTopic, Action<string> actionHandler);
    }

}
