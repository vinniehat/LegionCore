
namespace LegionCore.Core.Models.Misc
{
    public partial class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }

        public Logging Logging { get; set; }

        public string AllowedHosts { get; set; }

        public Jwt Jwt { get; set; }

        public Discord Discord { get; set; }
        public List<NavigationMenuItem> NavigationMenuItems { get; set; }
    }

    public partial class ConnectionStrings
    {
        public string DefaultConnectionDevelopment { get; set; }

        public string DefaultConnectionProduction { get; set; }

        public string DefaultConnectionStaging { get; set; }
    }

    public partial class Discord
    {
        public Bot Bot { get; set; }
    }

    public partial class Bot
    {
        public string Token { get; set; }

        public string Prefix { get; set; }
    }

    public partial class Jwt
    {
        public Uri ValidAudience { get; set; }

        public Uri ValidIssuer { get; set; }

        public string Secret { get; set; }
    }

    public partial class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public partial class LogLevel
    {
        public string Default { get; set; }

        public string MicrosoftAspNetCore { get; set; }
    }
}