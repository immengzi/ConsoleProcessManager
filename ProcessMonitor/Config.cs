using System.IO;
using System.Text.Json;

namespace ProcessMonitor
{
    class Config
    {
        // 是否开机自启
        public bool StartupEnabled { get; private set; }
        // 主题颜色选择
        public string ThemeColor { get; private set; }

        public void LoadConfig()
        {
            string configFile = "config.json";
            string configJson = File.ReadAllText(configFile);
            var config = JsonSerializer.Deserialize<Config>(configJson);

            StartupEnabled = config.StartupEnabled;
            ThemeColor = config.ThemeColor;
        }
    }
}