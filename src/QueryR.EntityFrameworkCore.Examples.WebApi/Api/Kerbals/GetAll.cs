using Microsoft.AspNetCore.Mvc;
using QueryR.EntityFrameworkCore.Examples.WebApi.Infrastructure.Database;
using QueryR.EntityFrameworkCore.Examples.WebApi.Shared.Constants;
using QueryR.EntityFrameworkCore.Examples.WebApi.Shared.Endpoints;
using static QueryR.EntityFrameworkCore.Examples.WebApi.Api.Kerbals.GetAll;

namespace QueryR.EntityFrameworkCore.Examples.WebApi.Api.Kerbals;

public class GetAll(IGetAllKerbalsService getAllKerbalsService) : ControllerBase
{
    [HttpGet(Routes.Api.Kerbals.Url)]
    public async Task<IActionResult> Action(
        [FromQuery(Name = "")] QueryParameters parameters) =>
            Ok(await getAllKerbalsService.GetAllAsync(parameters));

    public class ListWithTotalCount<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = new();
    }
    public interface IGetAllKerbalsService
    {
        Task<ListWithTotalCount<Kerbal>> GetAllAsync(QueryParameters parameters, CancellationToken cancellationToken = default);
    }
    public class GetAllKerbalsService(
        IQueryParametersMapper queryParametersMapper,
        KerbalDbContext kerbalDbContext
        ) : IGetAllKerbalsService
    {
        public readonly IQueryParametersMapper queryParametersMapper = queryParametersMapper;
        public readonly KerbalDbContext kerbalDbContext = kerbalDbContext;

        public async Task<ListWithTotalCount<Kerbal>> GetAllAsync(QueryParameters parameters, CancellationToken cancellationToken = default)
        {
            kerbalDbContext.Database.EnsureCreated();

            var query = queryParametersMapper.ToQuery(parameters);

            var (totalCount, kerbals) = await kerbalDbContext.Set<Kerbal>().Query(query)
                .GetCountAndListAsync(cancellationToken);

            //Normally you would use a DTO mapper here to prevent cyclic dependencies.
            //The ExampleRequests.http file has a request that uses sparse fields to avoid this for the purposes of the demo.
            return new ListWithTotalCount<Kerbal>
            {
                TotalCount = totalCount,
                Items = kerbals
            };
        }
    }
}
