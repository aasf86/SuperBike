using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperBike.Auth.Business;
using SuperBike.Business.Contracts.UseCases.Rent;
using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.Rent;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuperBike.Api.Controllers
{
    /// <summary>
    /// Controller para gestão de cadastros de alugueis de motocicletas.
    /// </summary>
#if !DEBUG
    [Authorize(Roles = RoleTypeSuperBike.RenterDeliveryman)]
#endif
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly IRentUseCase _rentUseCase;
        private IRentUseCase RentUseCase => _rentUseCase;

        /// <summary>
        /// Controller para gestão de cadastros de alugueis de motocicletas.
        /// </summary>
        /// <param name="rentUseCase"></param>
        public RentController(IRentUseCase rentUseCase)
        {
            _rentUseCase = rentUseCase;            
        }

        /// <summary>
        /// Inserir novo aluguel.
        /// </summary>
        /// <param name="rent"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] RentInsert rent)
        {
            if (ModelState.IsValid)
            {
                if (User is not null) Thread.CurrentPrincipal = new ClaimsPrincipal(User.Identity);

                var rentInsertRequest = RequestBase.New(rent, "host:api", "1.0");
                rent.SetUser(User.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault().Value);

                var renterInsertResponse = await RentUseCase.Insert(rentInsertRequest);

                if (renterInsertResponse.IsSuccess)
                    return Ok(renterInsertResponse);

                return BadRequest(renterInsertResponse);
            }

            return BadRequest();
        }

        /// <summary>
        /// Obter os alugueis.
        /// </summary>        
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (User is not null) Thread.CurrentPrincipal = new ClaimsPrincipal(User.Identity);

            var rentGet = new RentGet { UserId = User.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault().Value };
            var result = RentUseCase.Validate(rentGet);

            if (!result.IsSuccess) return BadRequest(result);

            var rentGetRequest = RequestBase.New(rentGet, "host:api", "1.0");
            var rentGetResponse = await RentUseCase.Get(rentGetRequest);

            if (rentGetResponse.IsSuccess && rentGetResponse.Data.Count() > 0)
                return Ok(rentGetResponse);

            return NotFound(rentGetResponse);            
        }
    }
}
