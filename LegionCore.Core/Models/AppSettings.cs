using Newtonsoft.Json;

namespace LegionCore.Core.Models
{
    public partial class AppSettings
    {
        [JsonProperty("ConnectionStrings")]
        public ConnectionStrings ConnectionStrings { get; set; }

        [JsonProperty("Logging")]
        public Logging Logging { get; set; }

        [JsonProperty("AllowedHosts")]
        public string AllowedHosts { get; set; }

        [JsonProperty("JWT")]
        public Jwt Jwt { get; set; }

        [JsonProperty("Discord")]
        public Discord Discord { get; set; }
    }

    public partial class ConnectionStrings
    {
        [JsonProperty("DefaultConnection.Development")]
        public string DefaultConnectionDevelopment { get; set; }

        [JsonProperty("DefaultConnection.Production")]
        public string DefaultConnectionProduction { get; set; }

        [JsonProperty("DefaultConnection.Staging")]
        public string DefaultConnectionStaging { get; set; }
    }

    public partial class Discord
    {
        [JsonProperty("Bot")]
        public Bot Bot { get; set; }
    }

    public partial class Bot
    {
        [JsonProperty("Token")]
        public string Token { get; set; }

        [JsonProperty("Prefix")]
        public string Prefix { get; set; }
    }

    public partial class Jwt
    {
        [JsonProperty("ValidAudience")]
        public Uri ValidAudience { get; set; }

        [JsonProperty("ValidIssuer")]
        public Uri ValidIssuer { get; set; }

        [JsonProperty("Secret")]
        public string Secret { get; set; }
    }

    public partial class Logging
    {
        [JsonProperty("LogLevel")]
        public LogLevel LogLevel { get; set; }
    }

    public partial class LogLevel
    {
        [JsonProperty("Default")]
        public string Default { get; set; }

        [JsonProperty("Microsoft.AspNetCore")]
        public string MicrosoftAspNetCore { get; set; }
    }
}