using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace QueryR.EntityFrameworkCore.Tests.TestHelpers
{
    public abstract class RqbBase<TDataBaseContext> : IDisposable where TDataBaseContext : DbContext
    {
        protected ITestOutputHelper? testOutputHelper;
        protected static DbContextOptions<TDataBaseContext> Options { get; private set; } = new DbContextOptionsBuilder<TDataBaseContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        private readonly SqliteConnection sqliteConnection;

        public RqbBase(ITestOutputHelper? testOutputHelper = null)
        {
            this.testOutputHelper = testOutputHelper;
            sqliteConnection = new SqliteConnection("DataSource=:memory:");
            sqliteConnection.Open();
            var optionsBuilder = new DbContextOptionsBuilder<TDataBaseContext>()
                .UseSqlite(sqliteConnection);
            if (testOutputHelper != null)
            {
                optionsBuilder = optionsBuilder.LogTo(testOutputHelper.WriteLine);
            }
            Options = optionsBuilder.Options;
            using var context = (TDataBaseContext)Activator.CreateInstance(typeof(TDataBaseContext), Options);
            context.Database.EnsureCreated();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
        public void Dispose()
        {
            if (sqliteConnection != null)
            {
                sqliteConnection.Close();
            }
        }
      
        public static IEnumerable<object[]> IsAscendings =>
            new[] { true, false }.Select(x => new object[] { x });
    }
}
