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
public class KerbalsGet : BaseController
{
    [HttpGet(Routes.Api.Kerbals.Url)]
    public async Task<IActionResult> Action(Command command) => await Mediator.Send(command);
    public class Command : IRequest<IActionResult>
    {
        [FromQuery(Name = "")]
        public QueryParameters QueryParameters { get; set; } = new();
    }
    public class ResponseObject
    {
        public int TotalCount { get; set; }
        public List<Kerbal> Kerbals { get; set; }
    }
    public class Handler : IRequestHandler<Command, IActionResult>
    {
        public readonly IQueryParameterMapper queryParameterMapper;
        public readonly KspDbContext kspDbContext;
        public Handler(
            IQueryParameterMapper queryParameterMapper,
            KspDbContext kspDbContext
        )
        {
            this.queryParameterMapper = queryParameterMapper;
            this.kspDbContext = kspDbContext;
        }

        public async Task<IActionResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var query = queryParameterMapper.MapToQuery(request.QueryParameters);

            var (totalCount, kerbals) = await kspDbContext.Set<Kerbal>().Query(query)
                .GetCountAndListAsync(cancellationToken);

            return new OkObjectResult(new ResponseObject {
                TotalCount = totalCount,
                Kerbals = kerbals
            });
        }
    }
}
```

