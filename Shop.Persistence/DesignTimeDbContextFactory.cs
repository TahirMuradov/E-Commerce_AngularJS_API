using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shop.Persistence.Context;

namespace Shop.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDBContext>
{
    public AppDBContext CreateDbContext(string[] args)
    {
      
       DbContextOptionsBuilder<AppDBContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseSqlServer(Configuration.DefaultConnectionString);
            return new(dbContextOptionsBuilder.Options);
    }
}
