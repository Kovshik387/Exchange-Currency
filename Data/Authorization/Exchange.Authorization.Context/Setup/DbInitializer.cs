using Exchange.Authorization.Context.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Authorization.Context.Setup;

public static class DbInitializer
{
    public static void Execute(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
        ArgumentNullException.ThrowIfNull(scope);

        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AuthorizationDbContext>>();
        using var context = dbContextFactory.CreateDbContext();
        context.Database.Migrate();
    }
}