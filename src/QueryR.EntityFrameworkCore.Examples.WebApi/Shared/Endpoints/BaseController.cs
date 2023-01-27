using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace QueryR.EntityFrameworkCore.Examples.WebApi.Shared.Endpoints
{
    public class BaseController : ControllerBase
    {
        public IMediator Mediator { get; set; } = null!;
    }
}
