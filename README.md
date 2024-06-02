# QueryR.EntityFrameworkCore

![QueryR Logo](./assets/logo.png)

[![.NET](https://github.com/craigmccauley/QueryR.EntityFrameworkCore/actions/workflows/dotnet.yml/badge.svg)](https://github.com/craigmccauley/QueryR.EntityFrameworkCore/actions/workflows/dotnet.yml)

QueryR.EntityFrameworkCore adds a few important items into [QueryR](https://github.com/craigmccauley/QueryR) for use with EntityFrameworkCore.

## Includes

QueryR.EntityFrameworkCore introduces a new `EfQuery` object that should be used for querying instead of the QueryR `Query` object.

The `EfQuery` object has an `Includes` member where the navigation property path can be set to include related entities with the query. It is important to use this setting for specifying Include statements as the values control the maximum search depth for the SparseFields.

## Async QueryResult Extensions

QueryR.EntityFrameworkCore would not be complete without new QueryResult extension methods to call the `CountAsync` and `ToListAsync` extension methods provided by EntityFrameworkCore.

## Web Api Example

```CSharp
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

```

