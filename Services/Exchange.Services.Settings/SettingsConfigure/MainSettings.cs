using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Services.Settings.SettingsConfigure
{
    public class MainSettings
    {
        public string PublicUrl { get; private set; } = null!;
        public string InternalUrl { get; private set; } = null!;
        public string AllowedOrigins { get; private set; } = "";
        public int UploadFileSizeLimit { get; private set; } = 20971520;
    }
}
