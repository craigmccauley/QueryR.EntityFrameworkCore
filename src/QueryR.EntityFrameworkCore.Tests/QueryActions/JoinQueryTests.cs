using QueryR.EntityFrameworkCore.Tests.TestHelpers;
using QueryR.QueryModels;
using Xunit.Abstractions;

namespace QueryR.EntityFrameworkCore.Tests.QueryActions
{
    [Collection("Database")]
    public class JoinQueryTests : RqbBase<TestDbContext>
    {
        public JoinQueryTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        internal async void Query_WhenQuerableIsJoinedObject_ShouldWorkAsExpected()
        {

            //arrange
            using var context = new TestDbContext(Options);

            var joinQuery = context.Set<Pet>()
                .Join(
                    context.Persons,
                    pet => pet.OwnerId,
                    owner => owner.Id,
                    (pet, owner) => new
                    {
                        PetName = pet.Name,
                        OwnerName = owner.Name,
                    });

            var filter = new Filter
            {
                PropertyName = "OwnerName",
                Operator = FilterOperators.Equal,
                Value = "Craig"
            };
            
            //act
            var (count, items) = await joinQuery.Query(filter).GetCountAndListAsync();

            //assert
            count.Should().BeGreaterThan(0);
            foreach(var item in items)
            {
                item.OwnerName.Should().Be("Craig");
            }
        }
    }
}
