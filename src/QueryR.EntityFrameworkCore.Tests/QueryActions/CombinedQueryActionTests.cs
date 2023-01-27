using QueryR.EntityFrameworkCore.QueryModels;
using QueryR.EntityFrameworkCore.Tests.TestHelpers;
using QueryR.QueryModels;
using Xunit.Abstractions;

namespace QueryR.EntityFrameworkCore.Tests.QueryActions
{
    [Collection("Database")]
    public class CombinedQueryActionTests : RqbBase<TestDbContext>
    {
        public CombinedQueryActionTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        internal void BuildIncludeAndSparseFields_ShouldWorkOnSingleRelatedData()
        {
            //arrange
            using var context = new TestDbContext(Options);

            var querySpec = new EfQuery
            {
                Includes = new List<Include>
                {
                    new Include{ NavigationPropertyPath = nameof(Pet.Owner) },
                },
                SparseFields = new List<SparseField>
                {
                    new SparseField
                    {
                        EntityName = nameof(Person),
                        PropertyNames = new List<string>{nameof(Person.Name) }
                    },
                    new SparseField
                    {
                        EntityName = nameof(Pet),
                        PropertyNames = new List<string>{ nameof(Pet.Name), nameof(Pet.Owner) }
                    }
                }
            };

            //act
            var result = context.Set<Pet>().Query(querySpec).PagedQuery;

            //assert
            result.Should().NotBeNullOrEmpty();
            foreach (var pet in result)
            {
                pet.Id.Should().Be(default);
                pet.Name.Should().NotBeNullOrWhiteSpace();
                pet.Owner.Should().NotBeNull();
                pet.Owner!.Id.Should().Be(default);
                pet.Owner!.Name.Should().NotBeNullOrWhiteSpace();
            }
        }

        [Fact]
        internal async Task BuildIncludeAndSparseFields_ShouldWorkOnListRelatedData()
        {
            //arrange
            using var context = new TestDbContext(Options);

            var querySpec = new EfQuery
            {
                Includes = new List<Include>
                {
                    new Include{ NavigationPropertyPath = nameof(Person.Pets) },
                },
                SparseFields = new List<SparseField>
                {
                    new SparseField
                    {
                        EntityName = nameof(Person),
                        PropertyNames = new List<string>{nameof(Person.Name), nameof(Person.Pets) }
                    },
                    new SparseField
                    {
                        EntityName = nameof(Pet),
                        PropertyNames = new List<string>{ nameof(Pet.Name) }
                    }
                }
            };

            //act
            var result = await context.Set<Person>().Query(querySpec).ToListAsync();

            //assert
            foreach (var person in result!)
            {
                person.Id.Should().Be(default);
                person.Name.Should().NotBeNullOrWhiteSpace();
                person.Age.Should().Be(default);
                person.Pets.Should().NotBeNullOrEmpty();
                foreach (var pet in person.Pets!)
                {
                    pet.Id.Should().Be(default);
                    pet.PetType.Should().Be(default);
                    pet.PetTypeId.Should().Be(default);
                    pet.OwnerId.Should().Be(default);
                    pet.Owner.Should().Be(default);
                    pet.Name.Should().NotBeNullOrWhiteSpace();
                }
            }
        }

        [Fact]
        internal async Task BuildSparseFields_WhenCircularReference_ShouldPopulateToCorrectDepth()
        {
            //arrange
            using var context = new TestDbContext(Options);

            var querySpec = new EfQuery
            {
                Includes = new List<Include>
                {
                    new Include
                    {
                         NavigationPropertyPath = $"{nameof(Person.Pets)}.{nameof(Pet.Owner)}.{nameof(Person.Pets)}",
                    },
                },
                SparseFields = new List<SparseField>
                {
                    new SparseField
                    {
                        EntityName = nameof(Person),
                        PropertyNames = new List<string>{nameof(Person.Name), nameof(Person.Pets) }
                    },
                    new SparseField
                    {
                        EntityName = nameof(Pet),
                        PropertyNames = new List<string>{ nameof(Pet.Name), nameof(Pet.Owner) }
                    }
                }
            };

            //act
            var result = await context.Set<Person>().Query(querySpec).ToListAsync();

            //assert
            foreach (var person in result)
            {
                person.Pets.Should().NotBeNullOrEmpty();
                foreach (var pet in person.Pets!)
                {
                    pet.Name.Should().NotBeNullOrWhiteSpace();
                    pet.Owner.Should().NotBeNull();
                    pet.Owner!.Pets.Should().NotBeNullOrEmpty();
                    foreach (var ownerPet in pet.Owner.Pets!)
                    {
                        ownerPet.Name.Should().NotBeNullOrWhiteSpace();
                        ownerPet.Owner.Should().BeNull();
                    }
                }
            }
        }
    }
}
