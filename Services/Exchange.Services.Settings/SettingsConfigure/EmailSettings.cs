using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Services.Settings.SettingsConfigure;

public class EmailSettings
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public int Port { get; set; } = default!;
}
