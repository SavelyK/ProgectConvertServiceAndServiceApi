using Microsoft.AspNetCore.Mvc;
using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace BalancingService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    }
}
