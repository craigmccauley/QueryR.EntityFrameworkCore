using MediatR;
using Microsoft.AspNetCore.Mvc;
using QueryR.EntityFrameworkCore.Examples.WebApi.Infrastructure.Database;
using QueryR.EntityFrameworkCore.Examples.WebApi.Shared.Constants;
using QueryR.EntityFrameworkCore.Examples.WebApi.Shared.Endpoints;

namespace QueryR.EntityFrameworkCore.Examples.WebApi.Features.Kerbals;

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
        public List<Kerbal> Kerbals { get; set; } = new();
    }
    public class Handler : IRequestHandler<Command, IActionResult>
    {
        public readonly IQueryParametersMapper queryParametersMapper;
        public readonly KerbalDbContext kerbalDbContext;
        public Handler(
            IQueryParametersMapper queryParametersMapper,
            KerbalDbContext kerbalDbContext
        )
        {
            this.queryParametersMapper = queryParametersMapper;
            this.kerbalDbContext = kerbalDbContext;
        }

        public async Task<IActionResult> Handle(Command request, CancellationToken cancellationToken)
        {
            kerbalDbContext.Database.EnsureCreated();

            var query = queryParametersMapper.ToQuery(request.QueryParameters);

            var (totalCount, kerbals) = await kerbalDbContext.Set<Kerbal>().Query(query)
                .GetCountAndListAsync(cancellationToken);

            return new OkObjectResult(new ResponseObject
            {
                TotalCount = totalCount,
                Kerbals = kerbals
            });
        }
    }
}
