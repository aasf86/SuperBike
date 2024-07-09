using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperBike.Auth.Business;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuperBike.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoController : ControllerBase
    {
        // GET: api/<GoController>
        [Authorize]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<GoController>/5
        [Authorize(Roles = RoleTypeSuperBike.Admin)]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GoController>
        [Authorize(Roles = RoleTypeSuperBike.Deliveryman)]
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
