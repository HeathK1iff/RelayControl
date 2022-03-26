using MqttDevices.Model;
using MqttDevices.Model.Client;
using MqttDevices.Model.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;

namespace MqttDevices
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string _configFileName;
        private MqttClientService _client;
        private DeviceCollection _devices;
        private Config _settings;
        public Config Settings { get => _settings; }
        public DeviceCollection Devices { get => _devices; }
        public string ConfigFileName { get => _configFileName; }

        public Window Active { get; set; }

        public void RefreshDevices()
        {
            var task = Task.Run(async() => await _devices.RefreshListAsync(_settings.DeviceDefenitions));
            task.Wait();
        }

        public async Task RefreshDevicesAsync()
        {
            await _devices.RefreshListAsync(_settings.DeviceDefenitions);
        }

        public async Task InitAsync()
        {
            _settings = await Config.LoadAsync(_configFileName);
            _client = new MqttClientService(_settings.Host);
            _devices = new DeviceCollection(_client);
            await RefreshDevicesAsync();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _client.Dispose();
            base.OnExit(e);
        }


        public Window GetActiveWindow()
        {

            return this.Windows.OfType<Window>().FirstOrDefault(f => f.IsActive);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var rootPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            _configFileName = Path.Combine(rootPath, "App.ini");
        }

    }
}
