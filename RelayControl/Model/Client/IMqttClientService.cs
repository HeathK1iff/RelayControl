using System.Threading;
using System.Threading.Tasks;

namespace MqttDevices.Model.Client
{
    public interface IMqttClientService
    {
        Task DisconnectAsync();
        bool IsConnected();
        void PublishAsync(string topic, string message);
        Task<bool> ConnectAsync(SubscriberDelegate subscriber = null, 
            CancellationToken? token = null);
    }
}
