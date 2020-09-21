using Newtonsoft.Json;

namespace DiscordPugBot
{
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("CommandPrefix")]
        public string Prefix { get; private set; }

        [JsonProperty("appId")]
        public string AppId { get; private set; }

        [JsonProperty("appSecret")]
        public string AppSecret { get; private set; }

    }
}
