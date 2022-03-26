using System;

namespace MqttDevices.Utils
{
    public interface ICommandEvents
    {
        public event Action OnSuccess;
        public event Action<Exception> OnError;
    }
}
