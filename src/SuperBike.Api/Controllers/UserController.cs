using Microsoft.AspNetCore.Mvc;
using SuperBike.Business.Contracts.UseCases.User;
using SuperBike.Business.Dtos.User;
using SuperBike.Business.Dtos.User.Request;

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
        public async Task<IActionResult> Login([FromBody] UserLogin user)
        {
            if (ModelState.IsValid)
            {
                var userLoginRequest = new UserLoginRequest(user);
                userLoginRequest.From = "host:api";
                userLoginRequest.Version = "1.0";
                var userLoginResponse = await UserUseCase.Login(userLoginRequest);

                if (userLoginResponse.IsSuccess)
                    return Ok(userLoginResponse);

                if (userLoginResponse.Exception != null)
                    return StatusCode(503, userLoginResponse);

                return BadRequest(userLoginResponse);
            }

            return BadRequest();
        }
    }
}
