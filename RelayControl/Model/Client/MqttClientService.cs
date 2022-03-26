using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace MqttDevices.Model.Client
{
    public delegate void SubscriberDelegate(ISubscriber subscriber);

    class MqttClientService : IMqttClientService, ISubscriber, IDisposable
    {
        private bool _disposed;
        private readonly MqttFactory _mqttFactory = new();
        private readonly IMqttClient _mqttClient;
        private CancellationTokenSource _cancelTokenSource;
        private CancellationToken? _token;
        private readonly List<(string Topic, Action<string> Handler)> _subscriptions = new();

        public MqttClientService(string host)
        {
            _mqttClient = _mqttFactory.CreateMqttClient();
            Host = host;
        }

        public string Host { get; set; }

        public async Task<bool> ConnectAsync(SubscriberDelegate subscriber = null, CancellationToken? token = null)
        {
            if (!_mqttClient.IsConnected)
            {
                if (String.IsNullOrEmpty(Host))
                    return _mqttClient.IsConnected;

                var options = new MqttClientOptionsBuilder()
                    .WithClientId(Guid.NewGuid().ToString())
                    .WithTcpServer(Host).Build();

                _token = token;
                if (_token is null)
                {
                    _cancelTokenSource = new CancellationTokenSource();
                    _token = _cancelTokenSource.Token;
                }
                else
                {
                    _cancelTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_token.Value);
                }

                _cancelTokenSource.Token.Register(async () =>
                {
                    if (IsConnected())
                        await _mqttClient.DisconnectAsync();
                });

                try
                {
                    try
                    {
                        subscriber?.Invoke(this);

                        var state = await _mqttClient.ConnectAsync(options, _token.Value);

                        if (state.ResultCode != MqttClientConnectResultCode.Success)
                        {
                            throw new Exception($"Connection is not established. Reason: {state.ReasonString}");
                        }

                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }

                    foreach (var subscription in _subscriptions)
                    {
                        var mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder()
                            .WithTopicFilter(f => { f.WithTopic(subscription.Topic); }).Build();

                        var result = await _mqttClient.SubscribeAsync(mqttSubscribeOptions, _cancelTokenSource.Token);
                    }

                    _mqttClient.UseApplicationMessageReceivedHandler((e) =>
                    {
                        string receivedTopic = e.ApplicationMessage.Topic;


                        foreach (var subscription in _subscriptions)
                        {
                            if ((subscription.Topic != "#") &&
                                (subscription.Topic != receivedTopic))
                                continue;

                            subscription.Handler?.Invoke(e.ApplicationMessage.ConvertPayloadToString());
                        }
                    });
                }
                finally
                {
                    _cancelTokenSource = null;
                }
            }
            return _mqttClient.IsConnected;
        }

        public async Task DisconnectAsync()
        {
            await _mqttClient.DisconnectAsync();
        }

        public bool IsConnected()
        {
            return _mqttClient.IsConnected;
        }

        public void Subscribe(string mqttTopic, Action<string> actionHandler)
        {
            if (IsConnected())
            {
                throw new Exception($"Topic {mqttTopic} can not registered because connection is established");
            }

            _subscriptions.Add((Topic: mqttTopic, Handler: actionHandler));
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _mqttClient.Dispose();
            }

            _disposed = true;
        }


        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async void PublishAsync(string topic, string message)
        {
            if (!IsConnected())
                return;

            await _mqttClient.PublishAsync(new MqttApplicationMessage()
            {
                Topic = topic,
                Payload = System.Text.Encoding.ASCII.GetBytes(message)
            });
        }

        ~MqttClientService()
        {
            Dispose(disposing: false);
        }

    }
}
