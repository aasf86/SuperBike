using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperBike.Auth.Business;
using SuperBike.Business.Contracts.UseCases.Motorcycle;
using SuperBike.Business.Dtos.Motorcycle;
using SuperBike.Business.Dtos.Motorcycle.Request;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuperBike.Api.Controllers
{
    //aasf86
    //[Authorize(Roles = RoleTypeSuperBike.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class MotorcycleController : ControllerBase
    {
        private readonly IMotorcycleUseCase _motorcycleUseCase;
        private IMotorcycleUseCase MotorcycleUseCase => _motorcycleUseCase;

        public MotorcycleController(IMotorcycleUseCase motorcycleUseCase) => _motorcycleUseCase = motorcycleUseCase;
        /*
        // GET: api/<GoController>        
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<GoController>/5        
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        */
        // POST api/<GoController>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="motorcycle"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] MotorcycleInsert motorcycle)
        {
            if (ModelState.IsValid)
            {
                var motorcycleInsertRequest = new MotorcycleInsertRequest(motorcycle);
                motorcycleInsertRequest.RequestId = Guid.NewGuid();
                motorcycleInsertRequest.From = "host:api";
                motorcycleInsertRequest.Version = "1.0";
                var motorcycleInsertResponse = await MotorcycleUseCase.Insert(motorcycleInsertRequest);

                if (motorcycleInsertResponse.IsSuccess)
                    return Ok(motorcycleInsertResponse);

                return BadRequest(motorcycleInsertResponse);
            }

            return BadRequest();
        }
        /*
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
        */
    }
}
