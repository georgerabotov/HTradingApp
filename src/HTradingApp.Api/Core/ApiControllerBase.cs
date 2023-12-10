using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HTradingApp.Api.Core
{
    public class ApiControllerBase : Controller
    {
        private readonly IMediator _mediator;
        public ApiControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<IActionResult> Ok<TResponse>(IRequest<TResponse> query)
        {
            var response = await _mediator.Send(query);
            return base.Ok(response);
        }

        protected async Task<IActionResult> NoContent<TResponse>(IRequest<TResponse> command)
        {
            await _mediator.Send(command);
            return base.NoContent();
        }
    }
}