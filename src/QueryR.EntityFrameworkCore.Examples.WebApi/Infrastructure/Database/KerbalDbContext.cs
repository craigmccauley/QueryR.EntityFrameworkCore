using Microsoft.EntityFrameworkCore;
using QueryR.EntityFrameworkCore.Examples.WebApi.Infrastructure.SampleDataGeneration;

namespace QueryR.EntityFrameworkCore.Examples.WebApi.Infrastructure.Database
{
    public class KerbalDbContext : DbContext
    {
        public KerbalDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Kerbal> Kerbals { get; set; }
        public DbSet<Craft> Crafts { get; set; }
        public DbSet<PlanetaryBody> PlanetaryBodies { get; set; }
        public DbSet<Snack> Snacks { get; set; }
        public DbSet<SnackType> SnacksType { get; set; }
        public DbSet<KerbalPlanetaryBody> KerbalPlanetaryBodies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SnackType>().HasData(SnackNames.Names.Select((name, index) =>
                new SnackType
                {
                    Id = index + 1,
                    Name = name,
                }));

            modelBuilder.Entity<PlanetaryBody>().HasData(PlanetaryBodyNames.Names.Select((name, index) =>
                new PlanetaryBody
                {
                    Id = index + 1,
                    Name = name,
                }));

            modelBuilder.Entity<Craft>().HasData(CraftNames.Names.Select((name, index) =>
                new Craft
                {
                    Id = index + 1,
                    CraftName = name,
                }));

            modelBuilder.Entity<Kerbal>().HasData(KerbalNames.Names.Select((name, index) => new Kerbal
            {
                Id = index + 1,
                FirstName = name,
                LastName = "Kerbin",
                AssignedSpaceCraftId = (index % CraftNames.Names.Count) + 1,
            }));

            var kerbalPlanetaryBodyId = 1;
            modelBuilder.Entity<KerbalPlanetaryBody>().HasData(KerbalNames.Names.SelectMany((name, kerbalIndex) =>
                Enumerable.Range(0, kerbalIndex % 4).Select(i => new KerbalPlanetaryBody
                {
                    Id = kerbalPlanetaryBodyId++,
                    KerbalId = kerbalIndex + 1,
                    PlanetaryBodyId = ((kerbalIndex + i) % PlanetaryBodyNames.Names.Count) + 1
                })));


            var snackId = 1;
            modelBuilder.Entity<Snack>().HasData(KerbalNames.Names.SelectMany((name, kerbalIndex) =>
                Enumerable.Range(0, kerbalIndex % 4).Select(i => new Snack
                {
                    Id = snackId++,
                    Amount = snackId % 4,
                    KerbalId = kerbalIndex + 1,
                    SnackTypeId = ((kerbalIndex + i) % SnackNames.Names.Count) + 1
                })));

        }
    }
}
