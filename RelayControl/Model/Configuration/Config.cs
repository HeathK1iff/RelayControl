using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System;

namespace MqttDevices.Model.Configuration
{
    public class Config
    {
        public string Host { get; set; }
        public List<DeviceDefenition> DeviceDefenitions { get; set; } = new List<DeviceDefenition>();

        public static async Task<Config> LoadAsync(string fileName)
        {
            if (!File.Exists(fileName))
                return new Config();

            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 2048, true))
            {
                if (stream.Length == 0)
                {
                    return new Config();
                }

                return await JsonSerializer.DeserializeAsync<Config>(stream);
            }
        }

        public async void SaveAsync(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Write, 2048, true))
            {
                await JsonSerializer.SerializeAsync<Config>(stream, this);
                await stream.FlushAsync();
            }   
        }
    }
}
