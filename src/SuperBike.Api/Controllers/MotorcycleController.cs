using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperBike.Auth.Business;
using SuperBike.Business.Contracts.UseCases.Motorcycle;
using SuperBike.Business.Dtos.Motorcycle;
using SuperBike.Business.Dtos.Motorcycle.Request;
using SuperBike.Business.UseCases.Motorcycle;

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

        */

        // POST api/<GoController>
        /// <summary>
        /// Inserir nova motocicleta.
        /// </summary>
        /// <param name="motorcycle"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] MotorcycleInsert motorcycle)
        {
            if (ModelState.IsValid)
            {
                var motorcycleInsertRequest = new MotorcycleInsertRequest(motorcycle);                
                motorcycleInsertRequest.From = "host:api";
                motorcycleInsertRequest.Version = "1.0";
                var motorcycleInsertResponse = await MotorcycleUseCase.Insert(motorcycleInsertRequest);

                if (motorcycleInsertResponse.IsSuccess)
                    return Ok(motorcycleInsertResponse);

                return BadRequest(motorcycleInsertResponse);
            }

            return BadRequest();
        }

        // GET api/<GoController>/5
        /// <summary>
        /// Obter uma motocicleta pela sua placa.
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        [HttpGet("{plate}")]
        public async Task<IActionResult> GetByPlate(string plate)
        {
            var motorcycle = new MotorcycleGet { Plate = plate };
            var result = MotorcycleUseCase.Validate(motorcycle);

            if (!result.IsSuccess) return BadRequest(result);

            var motorcycleGetRequest = new MotorcycleGetRequest(motorcycle);            
            motorcycleGetRequest.From = "host:api";
            motorcycleGetRequest.Version = "1.0";

            var motorcycleGetResponse = await MotorcycleUseCase.GetByPlate(motorcycleGetRequest);

            if (motorcycleGetResponse.IsSuccess && motorcycleGetResponse.Data.Id > 0) 
                return Ok(motorcycleGetResponse);

            return NotFound(motorcycleGetResponse);
        }

        // PUT api/<GoController>/5
        [HttpPut]
        public void Update([FromBody] MotorcycleUpdate motorcycle)
        {

        }

        /*
        // DELETE api/<GoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
