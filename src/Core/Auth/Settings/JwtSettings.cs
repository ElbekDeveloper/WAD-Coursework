using System;

namespace Core.Auth.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public TimeSpan TokenLifetime { get; set; }
    }
}
