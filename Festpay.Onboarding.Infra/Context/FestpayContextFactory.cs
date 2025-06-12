using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Festpay.Onboarding.Infra.Context;

public class FestpayContextFactory : IDesignTimeDbContextFactory<FestpayContext>
{
    public FestpayContext CreateDbContext()
    {
        string[] args = [""];
        return CreateDbContext(args);
    }

    public FestpayContext CreateDbContext(string[] args)
    {
        var connectionString = "Data Source=festpay.db" ?? throw new Exception("Connection string not found");

        // SQLite
        var optionsBuilder = new DbContextOptionsBuilder<FestpayContext>();
        optionsBuilder
            .UseSqlite(connectionString)
            .EnableSensitiveDataLogging(false)
            .EnableDetailedErrors(false);

        return new FestpayContext(optionsBuilder.Options);
    }
}
