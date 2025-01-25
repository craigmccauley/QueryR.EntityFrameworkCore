using QueryR.EntityFrameworkCore.QueryModels;
using QueryR.EntityFrameworkCore.Tests.TestHelpers;
using QueryR.QueryModels;

namespace QueryR.EntityFrameworkCore.Tests.QueryActions
{
    [Collection("Database")]
    public class SparseFieldsQueryActionTests : RqbBase<TestDbContext>
    {
        [Fact]
        internal async Task SparseFields_ShouldWorkAsExpected()
        {
            //arrange
            using var context = new TestDbContext(Options);
            var querySpec = new EfQuery
            {
                SparseFields = new List<SparseField>
                {
                    new SparseField
                    {
                        EntityName = nameof(Person),
                        PropertyNames = new List<string>
                        {
                            nameof(Person.Name),
                        }
                    },
                }
            };

            //act
            var result = await context.Set<Person>().Query(querySpec).ToListAsync();

            //assert
            foreach (var item in result)
            {
                item.Name.ShouldNotBe(default);
                item.Age.ShouldBe(default);
                item.Id.ShouldBe(default);
                item.Pets.ShouldBeNull();
            }
        }
    }
}
