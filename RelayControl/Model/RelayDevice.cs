using MqttDevices.Model.Client;
using MqttDevices.Model.Responses;
using MqttDevices.Utils;
using System.Text.Json;

namespace MqttDevices.Model
{
    public delegate void RelayStateChanged(RelayDevice device);
    public class RelayDevice : Device
    {
        private bool _state;
        public RelayDevice(IMqttClientService client) : base(client) { }

        protected override void DoSubscribe(ISubscriber subscriber)
        {
            subscriber.Subscribe(TopicHelper.Combine(Topic, "event/change_state"), (message) => 
            {
                var stateObj = JsonSerializer.Deserialize<RelayStateResponse>(message, new JsonSerializerOptions()
                                { PropertyNameCaseInsensitive = true});

                _state = stateObj.State == "ON";

                RelayStateChanged?.Invoke(this);
            });
        }

        public bool State => _state;

        public event RelayStateChanged RelayStateChanged;

        public void Switch(bool state)
        {
            _client.PublishAsync(TopicHelper.Combine(Topic, "relay/set_state"), state ? "on" : "off");
            _state = state;
        } 
    }
}
