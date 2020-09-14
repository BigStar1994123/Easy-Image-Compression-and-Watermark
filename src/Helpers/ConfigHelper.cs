using System.IO;
using System.Text.Json;

namespace ImageCompression.Helpers
{
    public static class ConfigHelper
    {
        public static Config ReadConfigFile()
        {
            var jsonString = File.ReadAllText("config.json");
            return JsonSerializer.Deserialize<Config>(jsonString);
        }
    }
}