using Microsoft.AspNetCore.Mvc;
using SuperBike.Business.Contracts.UseCases.User;
using SuperBike.Business.Dtos.User;
using SuperBike.Business.Dtos.User.Request;
using SuperBike.Business.Dtos.User.Response;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuperBike.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserUseCase _userUseCase;
        private IUserUseCase UserUseCase => _userUseCase;

        public UserController(IUserUseCase userUseCase) => _userUseCase = userUseCase;
        
        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] UserInsert user)
        {
            if (ModelState.IsValid)
            {
                var userInsertRequest = new UserInsertRequest(user);
                userInsertRequest.From = "host:api";
                userInsertRequest.Version = "1.0";
                var userInsertResponse = await UserUseCase.Insert(userInsertRequest);
                
                if (userInsertResponse.IsSuccess)                
                    return Ok(userInsertResponse);

                if (userInsertResponse.Exception != null)                
                    return StatusCode(503, userInsertResponse);

                return BadRequest(userInsertResponse);
            }

            return BadRequest();
        }

        // PUT api/<UserController>/5
        [HttpPost("Login")]
        public void Login([FromBody] UserInsert user)
        {

        }
    }
}
