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
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(2);
            result[0].Name.ShouldBe("Titan");
            result[1].Name.ShouldBe("Meowswers");
        }
    }
}
