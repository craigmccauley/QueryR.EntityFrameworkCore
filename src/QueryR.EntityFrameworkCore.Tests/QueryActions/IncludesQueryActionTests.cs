using QueryR.EntityFrameworkCore.QueryModels;
using QueryR.EntityFrameworkCore.Tests.TestHelpers;

namespace QueryR.EntityFrameworkCore.Tests.QueryActions
{
    [Collection("Database")]
    public class IncludesQueryActionTests : RqbBase<TestDbContext>
    {
        [Fact]
        internal void Include_WhenNoInclude_ShouldNotIncludeExtraData()
        {
            //arrange
            using var context = new TestDbContext(Options);

            //act
            var result = context.Set<Person>().Query(new EfQuery()).PagedQuery;

            //assert
            foreach (var person in result)
            {
                person.Pets.ShouldBeNull();
            }
        }

        [Fact]
        internal void Include_WhenOneLevelSpecified_ShouldReturnOneLevel()
        {
            //arrange
            using var context = new TestDbContext(Options);
            var querySpec = new EfQuery
            {
                Includes = new List<Include>
                {
                    new Include
                    {
                        NavigationPropertyPath = nameof(Person.Pets)
                    }
                }
            };

            //act
            var result = context.Set<Person>().Query(querySpec).PagedQuery;

            //assert
            foreach (var person in result)
            {
                person.Pets.ShouldNotBeNull();
                person.Pets.ShouldNotBeEmpty();
                foreach (var pet in person.Pets)
                {
                    pet.PetType.ShouldBeNull();
                }
            }
        }

        [Fact]
        internal void Include_WhenTwoLevelsSpecified_ShouldReturnTwoLevels()
        {
            //arrange
            using var context = new TestDbContext(Options);
            var querySpec = new EfQuery
            {
                Includes = new List<Include>
                {
                    new Include
                    {
                        NavigationPropertyPath = string.Join(".", nameof(Person.Pets), nameof(Pet.PetType))
                    }
                }
            };

            //act
            var result = context.Set<Person>().Query(querySpec).PagedQuery;

            //assert
            foreach (var person in result)
            {
                person.Pets.ShouldNotBeNull();
                person.Pets.ShouldNotBeEmpty();
                foreach (var pet in person.Pets)
                {
                    pet.PetType.ShouldNotBeNull();
                    (pet.PetType?.Name).ShouldNotBeNullOrWhiteSpace();
                }
            }
        }
    }
}
