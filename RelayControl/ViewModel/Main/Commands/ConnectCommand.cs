using MqttDevices.Model;
using MqttDevices.Model.Client;
using MqttDevices.Utils;

namespace MqttDevices.ViewModel.Main.Commands
{
    public partial class ConnectCommand : GenericCommand
    {       
        public ConnectCommand()
        {

        }

        void ConnectAsync()
        {
            //try
            //{
            //    await _service.ConnectAsync(_settigns.Host, subscriber =>
            //    {
                    //subscriber.Subscribe(_settigns.ChargerTopic, msg =>
                    //{
                    //    var stateObj = JsonSerializer.Deserialize<RelayStateResponse>(msg, new JsonSerializerOptions()
                    //    {
                    //        PropertyNameCaseInsensitive = true
                    //    });

                    //    Charger.ToggleState = stateObj.State == "ON";
                    //});
                    //subscriber.Subscribe(_settigns.HeaterTopic, msg =>
                    //{
                    //    var stateObj = JsonSerializer.Deserialize<RelayStateResponse>(msg, new JsonSerializerOptions()
                    //    {
                    //        PropertyNameCaseInsensitive = true
                    //    });

                    //    Heater.ToggleState = stateObj.State == "ON";
                    //});

            //    }).ContinueWith((isConnected) =>
            //    {
            //        canExecute = !isConnected.Result;
            //        Heater.Enabled = isConnected.Result;
            //        Charger.Enabled = isConnected.Result;
            //        OnSuccess?.Invoke();
            //    });
            //} catch (Exception e)
            //{
            //    OnError?.Invoke(e);
            //}
        }

        protected override void DoExecute(object parameter)
        {
            ConnectAsync();
        }
    }
}
