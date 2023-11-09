using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WFCustomization.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator Mediator;

        protected BaseController(IMediator mediator)
            => Mediator = mediator;

        protected IActionResult Select<TData>(TData data)
            where TData: class
                => data is null ? NotFound()
                                : Ok(data) as IActionResult;
    }
}
