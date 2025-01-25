using QueryR.EntityFrameworkCore.QueryModels;
using QueryR.EntityFrameworkCore.Tests.TestHelpers;
using QueryR.QueryModels;

namespace QueryR.EntityFrameworkCore.Tests.QueryActions
{
    [Collection("Database")]
    public class SortQueryActionTests : RqbBase<TestDbContext>
    {
        [Theory, MemberAutoData(nameof(IsAscendings))]
        internal async Task Sort_ShouldWorkAsExpected(
               bool isAscending)
        {
            //arrange
            using var context = new TestDbContext(Options);
            var querySpec = new EfQuery
            {
                Sorts = new List<Sort>
                {
                    new Sort
                    {
                        IsAscending = isAscending,
                        PropertyName = nameof(Person.Name)
                    },
                }
            };

            //act
            var result = await context.Set<Person>().Query(querySpec).ToListAsync();

            //assert
            if (isAscending)
            {
                result.Select(item => item.Name).ShouldBeInOrder();
            }
            else
            {
                result.Select(item => item.Name).Reverse().ShouldBeInOrder();
            }
        }
    }
}
