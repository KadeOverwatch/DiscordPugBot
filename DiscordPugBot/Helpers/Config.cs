using System.Configuration;

namespace DiscordPugBot.Helpers
{
    public static class BotConfig
    {
        public static System.Collections.Specialized.NameValueCollection appSettings = ConfigurationManager.AppSettings;
        public static string Get(string keyName)
        {
            if (DiscordPugBot.Properties.Settings.Default[keyName] != null)
            {
                return DiscordPugBot.Properties.Settings.Default[keyName].ToString();
            } else
            {
                return "Unknown";
            }
        }
    }
}
