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

        /// <summary>
        /// Controllers para gestão de usuários.
        /// </summary>
        /// <param name="userUseCase"></param>
        public UserController(IUserUseCase userUseCase) => _userUseCase = userUseCase;

        // POST api/<UserController>
        /// <summary>
        /// "Inserir um novo usuário locador."
        /// </summary>
        /// <param name="user"> parametro user</param>
        /// <returns> retorno aqui</returns>        
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

                return BadRequest(userInsertResponse);
            }

            return BadRequest();
        }

        // PUT api/<UserController>/5
        /// <summary>
        /// "Login para obtenção de token para usuários 'locador' e 'admin'."
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>        
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

                return BadRequest(userLoginResponse);
            }

            return BadRequest();
        }
    }
}
