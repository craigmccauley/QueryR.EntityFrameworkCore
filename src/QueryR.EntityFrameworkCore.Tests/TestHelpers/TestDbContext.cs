using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QueryR.EntityFrameworkCore.Tests.TestHelpers
{
    public class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public List<Pet>? Pets { get; set; }
    }
    public class Pet
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int OwnerId { get; set; }
        public Person? Owner { get; set; }
        public int PetTypeId { get; set; }
        public PetType? PetType { get; set; }
    }
    public class PetType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }


    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetType> PetTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var petTypes = new List<PetType>
            {
                new PetType { Id = 1, Name = "Cat" },
                new PetType { Id = 2, Name = "Dog" },
            };
            var persons = new List<Person>
            {
                new Person { Id = 1, Name = "Craig" },
                new Person { Id = 2, Name = "Also Craig" },
            };
            var pets = new List<Pet>
            {
                new Pet { Id = 1, Name = "Titan", OwnerId = 1, PetTypeId = 2 },
                new Pet { Id = 2, Name = "Rufus", OwnerId = 2, PetTypeId = 2 },
                new Pet { Id = 3, Name = "Meowswers" , OwnerId = 1, PetTypeId = 1 },
                new Pet { Id = 4, Name = "Kitty", OwnerId = 2, PetTypeId = 2 },
            };
            modelBuilder.Entity<PetType>(ba => ba.HasData(petTypes));
            modelBuilder.Entity<Person>(ba => ba.HasData(persons));
            modelBuilder.Entity<Pet>(ba => ba.HasData(pets));

            base.OnModelCreating(modelBuilder);
        }
    }
    public class TestDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
    {
        public TestDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseSqlite("DataSource=:memory:");
            return new TestDbContext(optionsBuilder.Options);
        }
    }
}
