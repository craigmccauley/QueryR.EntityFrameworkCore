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
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            foreach (var pet in result)
            {
                pet.Id.ShouldBe(default);
                pet.Name.ShouldNotBeNullOrWhiteSpace();
                pet.Owner.ShouldNotBeNull();
                pet.Owner!.Id.ShouldBe(default);
                pet.Owner!.Name.ShouldNotBeNullOrWhiteSpace();
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
                person.Id.ShouldBe(default);
                person.Name.ShouldNotBeNullOrWhiteSpace();
                person.Age.ShouldBe(default);
                person.Pets.ShouldNotBeNull();
                person.Pets.ShouldNotBeEmpty();
                foreach (var pet in person.Pets!)
                {
                    pet.Id.ShouldBe(default);
                    pet.PetType.ShouldBe(default);
                    pet.PetTypeId.ShouldBe(default);
                    pet.OwnerId.ShouldBe(default);
                    pet.Owner.ShouldBe(default);
                    pet.Name.ShouldNotBeNullOrWhiteSpace();
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
                person.Pets.ShouldNotBeNull();
                person.Pets.ShouldNotBeEmpty();
                foreach (var pet in person.Pets!)
                {
                    pet.Name.ShouldNotBeNullOrWhiteSpace();
                    pet.Owner.ShouldNotBeNull();
                    pet.Owner!.Pets.ShouldNotBeNull();
                    pet.Owner!.Pets.ShouldNotBeEmpty();
                    foreach (var ownerPet in pet.Owner.Pets!)
                    {
                        ownerPet.Name.ShouldNotBeNullOrWhiteSpace();
                        ownerPet.Owner.ShouldBeNull();
                    }
                }
            }
        }
    }
}
