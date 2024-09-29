namespace Exchange.Authorization.Context.Settings;

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

public enum DbType
{
    MSSQL = 0,
    PgSql = 1,
    MySql = 2
}
