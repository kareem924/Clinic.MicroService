using System.Collections.Generic;
using System.Threading.Tasks;
using Common.General.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security.API.Application.Queries.GetUserByUserName;
using Security.API.Application.Queries.GetUserPagedResult;
using Security.Core.Entities;

namespace Security.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public ActionResult<string> Get(int id)
        {
            return "value";
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