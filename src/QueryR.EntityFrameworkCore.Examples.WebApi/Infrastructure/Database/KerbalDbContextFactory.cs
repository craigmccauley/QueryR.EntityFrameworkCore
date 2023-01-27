using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace QueryR.EntityFrameworkCore.Examples.WebApi.Infrastructure.Database
{
    /// <summary>
    /// This factory is used by the EF Code First generator.
    /// See https://docs.microsoft.com/en-ca/ef/core/miscellaneous/cli/dbcontext-creation
    /// for details.
    /// </summary>
    public class KerbalDbContextFactory : IDesignTimeDbContextFactory<KerbalDbContext>
    {
        public KerbalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KerbalDbContext>();
            optionsBuilder.UseSqlite("DataSource=example.db");
            return new KerbalDbContext(optionsBuilder.Options);
        }
    }
}
