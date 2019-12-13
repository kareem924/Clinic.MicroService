using System;

using System.Threading.Tasks;
using Common.General.Entity;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ManageController> _logger;

        public ManageController(IMediator mediator, ILogger<ManageController> logger)
        {
            _mediator = mediator;
            _logger = logger;
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
        public void Post([FromBody] string value)
        {

        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}