using System;

namespace MqttDevices.Model.Configuration
{
    public class DeviceDefenition
    {
        private string _id;

        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                    _id = Guid.NewGuid().ToString();
                return _id;
            }
            set => _id = value;
        }

        public string Name { get; set; }
        public string Topic { get; set; }
    }
}
