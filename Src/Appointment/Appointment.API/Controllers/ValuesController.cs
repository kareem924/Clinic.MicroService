using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointment.Core.Entities;
using Appointment.Core.Enums;
using Appointment.Core.Repositories;
using Common.General.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointment.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IRepository<Session> _sessionRepositry;
        public ValuesController(IRepository<Session> sessionRepositry)
        {
            _sessionRepositry = sessionRepositry;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Session>>> Get()
        {
            await _sessionRepositry.AddAsync(new Session(SessionStatus.Canceled, DateTime.Today, TimeSpan.FromHours(1)));
            var sessions = await _sessionRepositry.GetAllAsync();
            var user = User.Claims.Select(x => new { x.Type, x.Value });
            return sessions.ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
