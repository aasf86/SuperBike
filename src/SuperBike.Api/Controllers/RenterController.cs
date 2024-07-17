﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperBike.Auth.Business;
using SuperBike.Business.Contracts.UseCases.Renter;
using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.Renter;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuperBike.Api.Controllers
{
    /// <summary>
    /// Controller para gestão de cadastros de entregador/alugador.
    /// </summary>
    //aasf86 Authorize
    //[Authorize(Roles = RoleTypeSuperBike.RenterDeliveryman)]
    [Route("api/[controller]")]
    [ApiController]
    public class RenterController : ControllerBase
    {
        private readonly IRenterUseCase _renterUseCase;
        private IRenterUseCase RenterUseCase => _renterUseCase;

        /// <summary>
        /// Controller para gestão de cadastros de entregador/alugador.
        /// </summary>
        /// <param name="renterUseCase"></param>
        public RenterController(IRenterUseCase renterUseCase)
        {
            _renterUseCase = renterUseCase;
            Thread.CurrentPrincipal = User;
            //Thread.CurrentPrincipal = new ClaimsPrincipal(User.Identity);
        }

        /// <summary>
        /// Inserir novo entregador/alugador.
        /// </summary>
        /// <param name="renter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] RenterInsert renter)
        {
            if (ModelState.IsValid)
            {
                var renterInsertRequest = RequestBase.New(renter, "host:api", "1.0");                
                var renterInsertResponse = await RenterUseCase.Insert(renterInsertRequest);

                if (renterInsertResponse.IsSuccess)
                    return Ok(renterInsertResponse);

                return BadRequest(renterInsertResponse);
            }

            return BadRequest();
        }

        /// <summary>
        /// Atualizar a foto da CNH do entregador/alugador.
        /// </summary>
        /// <param name="renter"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RenterUpdate renter)
        {
            if (ModelState.IsValid)
            {
                var renterUpdateRequest = RequestBase.New(renter, "host:api", "1.0");
                var renterUpdateResponse = await RenterUseCase.Update(renterUpdateRequest);

                if (renterUpdateResponse.IsSuccess)
                    return Ok(renterUpdateResponse);

                return BadRequest(renterUpdateResponse);
            }
            return BadRequest();
        }      
    }
}