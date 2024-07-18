using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperBike.Auth.Business;
using SuperBike.Business.Contracts.UseCases.Motorcycle;
using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.Motorcycle;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuperBike.Api.Controllers
{
    /// <summary>
    /// Controller para gestão de cadastros de motocicletas.
    /// </summary>
#if !DEBUG
    [Authorize(Roles = RoleTypeSuperBike.Admin)]
#endif
    [Route("api/[controller]")]
    [ApiController]
    public class MotorcycleController : ControllerBase
    {
        private readonly IMotorcycleUseCase _motorcycleUseCase;
        private IMotorcycleUseCase MotorcycleUseCase => _motorcycleUseCase;

        /// <summary>
        /// Controller para gestão de cadastros de motocicletas.
        /// </summary>
        /// <param name="motorcycleUseCase"></param>
        public MotorcycleController(IMotorcycleUseCase motorcycleUseCase)
        {
            _motorcycleUseCase = motorcycleUseCase;
        }
        
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
                if (User is not null) Thread.CurrentPrincipal = new ClaimsPrincipal(User.Identity);

                var motorcycleInsertRequest = RequestBase.New(motorcycle, "host:api", "1.0");                
                var motorcycleInsertResponse = await MotorcycleUseCase.Insert(motorcycleInsertRequest);

                if (motorcycleInsertResponse.IsSuccess)
                    return Ok(motorcycleInsertResponse);

                return BadRequest(motorcycleInsertResponse);
            }

            return BadRequest();
        }
                
        /// <summary>
        /// Obter uma motocicleta pela sua placa.
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        [HttpGet("{plate}")]
        public async Task<IActionResult> GetByPlate(string plate)
        {
            var motorcycleGet = new MotorcycleGet { Plate = plate };
            var result = MotorcycleUseCase.Validate(motorcycleGet);

            if (!result.IsSuccess) return BadRequest(result);

            var motorcycleGetRequest = RequestBase.New(motorcycleGet, "host:api", "1.0");
            var motorcycleGetResponse = await MotorcycleUseCase.GetByPlate(motorcycleGetRequest);

            if (motorcycleGetResponse.IsSuccess && motorcycleGetResponse.Data.Id > 0) 
                return Ok(motorcycleGetResponse);

            return NotFound(motorcycleGetResponse);
        }
        
        /// <summary>
        /// Atualizar a placa de uma motocicleta.
        /// </summary>
        /// <param name="motorcycle"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MotorcycleUpdate motorcycle)
        {
            if (ModelState.IsValid)
            {
                var motorcycleUpdateRequest = RequestBase.New(motorcycle, "host:api", "1.0");
                var motorcycleUpdateResponse = await MotorcycleUseCase.Update(motorcycleUpdateRequest);

                if (motorcycleUpdateResponse.IsSuccess)
                    return Ok(motorcycleUpdateResponse);

                return BadRequest(motorcycleUpdateResponse);
            }
            return BadRequest();
        }
        
        /// <summary>
        /// Remove motocicleta pelo seu 'Id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var motorcycleDelete = new MotorcycleDelete { Id = id };
            var result = MotorcycleUseCase.Validate(motorcycleDelete);

            if (!result.IsSuccess) return BadRequest(result);

            var motorcycleDeleteRequest = RequestBase.New(motorcycleDelete, "host:api", "1.0");
            var motorcycledeleteResponse = await MotorcycleUseCase.Delete(motorcycleDeleteRequest);

            if (motorcycledeleteResponse.IsSuccess)
                return Ok(motorcycledeleteResponse);

            return BadRequest(motorcycledeleteResponse);
        }        
    }
}
