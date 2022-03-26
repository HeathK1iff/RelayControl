using MqttDevices.Model.Client;
using MqttDevices.Model.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MqttDevices.Model
{
    public class DeviceCollection : List<Device>
    {
        private readonly IMqttClientService _client;

        public DeviceCollection(IMqttClientService client)
        {
            _client = client;
        }

        public async Task RefreshListAsync(List<DeviceDefenition> devices)
        {
            await DisconnectAsync();
            Clear();
            AddRange(devices.Select(f => new RelayDevice(_client) { Topic = f.Topic, Name = f.Name }).ToList<Device>());
            await ConnectAsync();
        }

        public async Task ConnectAsync()
        {
            await _client.ConnectAsync((subscriber)=> {
                foreach (IDevice device in this)
                    device.Subscribe(subscriber);
            });
        }

        public async Task DisconnectAsync()
        {
            await _client.DisconnectAsync();
        }


    }
}
