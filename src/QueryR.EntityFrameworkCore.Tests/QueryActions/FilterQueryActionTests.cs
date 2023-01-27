using QueryR.EntityFrameworkCore.QueryModels;
using QueryR.EntityFrameworkCore.Tests.TestHelpers;
using QueryR.QueryModels;

namespace QueryR.EntityFrameworkCore.Tests.QueryActions
{
    [Collection("Database")]
    public class FilterQueryActionTests : RqbBase<TestDbContext>
    {
        [Fact]
        internal async Task Filter_WhenFilterIsOnNavigationPropertyPath_ShouldFilterAsExpected()
        {
            //arrange
            using var context = new TestDbContext(Options);

            var querySpec = new EfQuery
            {
                Filters = new List<Filter>
                {
                    new Filter
                    {
                        PropertyName = $"{nameof(Pet.Owner)}.{nameof(Person.Name)}",
                        Operator = FilterOperators.StartsWith,
                        Value = "C"
                    },
                }
            };


            //act
            var result = await context.Set<Pet>().Query(querySpec).ToListAsync();

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(2);
            result[0].Name.Should().Be("Titan");
            result[1].Name.Should().Be("Meowswers");
        }
    }
}
