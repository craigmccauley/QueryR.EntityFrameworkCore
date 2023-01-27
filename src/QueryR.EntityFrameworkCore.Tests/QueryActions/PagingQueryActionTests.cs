using QueryR.EntityFrameworkCore.QueryModels;
using QueryR.EntityFrameworkCore.Tests.TestHelpers;
using QueryR.QueryModels;

namespace QueryR.EntityFrameworkCore.Tests.QueryActions
{
    [Collection("Database")]
    public class PagingQueryActionTests : RqbBase<TestDbContext>
    {
        [Fact]
        internal async Task Paging_ShouldWorkAsExpected()
        {
            //arrange
            using var context = new TestDbContext(Options);
            var querySpec = new EfQuery
            {
                Sorts = new List<Sort>
                {
                    new Sort { IsAscending = true, PropertyName = nameof(Person.Name) },
                },
                PagingOptions = new PagingOptions { PageNumber = 2, PageSize = 1 }
            };

            //act
            var (Count, Items) = await context.Set<Person>().Query(querySpec).GetCountAndListAsync();

            //assert
            Count.Should().Be(2);
            Items.Count.Should().Be(1);
            Items.First().Name.Should().Be("Craig");
        }
    }
}
