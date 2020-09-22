using System.Configuration;

namespace DiscordPugBot.Helpers
{
    public static class BotConfig
    {
        public static System.Collections.Specialized.NameValueCollection appSettings = ConfigurationManager.AppSettings;
        public static string Get(string keyName)
        {
            if (appSettings.Count > 0)
            {
                return appSettings[keyName];
            } else
            {
                return "Unknown";
            }
        }

        public static void Set(string keyName, string keyValue)
        {
            if (appSettings[keyName] == null)
            {
                appSettings.Add(keyName, keyValue);
            } else
            {
                appSettings[keyName] = keyValue;
            }
        }
    }
}
