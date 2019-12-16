using System;
using System.Threading.Tasks;
using Common.General.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Security.Infrastructure.Application.Commands.ActivateUserCommand;
using Security.Infrastructure.Application.Commands.ConfirmUserEmailCommand;
using Security.Infrastructure.Application.Commands.CreateUserCommand;
using Security.Infrastructure.Application.Commands.DeleteUserCommand;
using Security.Infrastructure.Application.Commands.UpdateUserCommand;
using Security.Infrastructure.Application.Dto;
using Security.Infrastructure.Application.Queries.GetUserDtoId;
using Security.Infrastructure.Application.Queries.GetUserPagedResult;

namespace Security.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ManageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<UserDto>>> Get([FromQuery] GetUserPagedResultQuery query)
        {
            var users = await _mediator.Send(query);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var users = await _mediator.Send(new GetUserDtoByIdQuery(id));
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserCommand input)
        {
            await _mediator.Publish(input);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateUserCommand input)
        {
            input.Id = id;
            await _mediator.Publish(new GetUserDtoByIdQuery(id));
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletePackage = new DeleteUserCommand { Id = id };
            await _mediator.Publish(deletePackage);
            return Ok();
        }

        [HttpPatch("{id}/ActivateUser")]
        public async Task<IActionResult> ActivateUser(Guid id, ActivateUserCommand input)
        {
            input.Id = id;
            await _mediator.Publish(input);
            return Ok();
        }

        [HttpPatch("{id}/ConfirmUserEmail")]
        public async Task<IActionResult> ConfirmUserEmail(Guid id, ConfirmUserEmailCommand input)
        {
            input.Id = id;
            await _mediator.Publish(input);
            return Ok();
        }
    }
}