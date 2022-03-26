namespace MqttDevices.Utils
{
    public class TopicHelper
    {
        public static string Combine(params string[] pathes)
        {
            string result = string.Empty;
            foreach (var path in pathes)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += '/';
                }

                result += path.TrimEnd(',', '/');
            }
            return result.Trim();
        }
    }
}
