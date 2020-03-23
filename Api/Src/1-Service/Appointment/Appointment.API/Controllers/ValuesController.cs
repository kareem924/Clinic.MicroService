using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointment.Core.Entities;
using Appointment.Core.Enums;
using Common.General.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Appointment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IRepository<Session> _sessionRepositry;
        private readonly ILogger<ValuesController> _logger;
        public ValuesController(IRepository<Session> sessionRepositry, ILogger<ValuesController> logger)
        {
            _sessionRepositry = sessionRepositry;
            _logger = logger;
        }
        // GET api/values
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Session>>> Get()
        //{
        //    await _sessionRepositry.AddAsync(new Session(SessionStatus.Canceled, DateTime.Today, TimeSpan.FromHours(1)));
        //    var sessions = await _sessionRepositry.GetAllAsync();
        //    var user = User.Claims.Select(x => new { x.Type, x.Value });
        //    _logger.LogInformation("test");
        //    return sessions.ToArray();
        //}

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
