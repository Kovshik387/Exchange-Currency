using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Services.Settings.SettingsConfigure
{
    public class SwaggerSettings
    {
        public bool Enabled { get; private set; } = false;

        public string OAuthClientId { get; private set; } = string.Empty;
        public string OAuthClientSecret { get; private set; } = string.Empty;
        public string Style { get; private set; } = string.Empty;

        public SwaggerSettings()
        {
            Enabled = false;
        }
    }
}
