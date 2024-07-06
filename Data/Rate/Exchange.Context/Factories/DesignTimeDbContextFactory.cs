using Exchange.Context.Context;
using Exchange.Context.Settings;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Context.Factories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RateDbContext>
    {
        public RateDbContext CreateDbContext(string[] args) 
        {
            var provider = (args?[0] ?? $"{DbType.MSSQL}").ToLower();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.context.json"), false)
                .Build();

            var connStr = configuration.GetConnectionString(provider);

            DbType dbType;
            if (provider.Equals($"{DbType.MSSQL}".ToLower()))
                dbType = DbType.MSSQL;
            else if (provider.Equals($"{DbType.PgSql}".ToLower()))
                dbType = DbType.PgSql;
            else if (provider.Equals($"{DbType.MySql}".ToLower()))
                dbType = DbType.MySql;
            else
                throw new Exception($"Unsupported provider: {provider}");

            var options = DbContextOptionsFactory.Create(connStr, dbType, false);
            var factory = new DbContextFactory(options);
            var context = factory.Create();

            return context;
        }
    }
}
