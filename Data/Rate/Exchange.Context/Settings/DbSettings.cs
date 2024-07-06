using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Context.Settings;
public class DbSettings
{
    public DbType DatabaseType { get; private set; }
    public string ConnectionString { get; private set; } = string.Empty;
    public DbInitSettings? InitSettings { get; private set; }
}

public class DbInitSettings
{
    public bool AddDemoData { get; private set; }
}
