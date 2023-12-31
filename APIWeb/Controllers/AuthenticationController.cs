﻿using APIWeb.Interfaces.Service;
using APIWeb.Models.Constants;
using APIWeb.Models.Identity;
using APIWeb.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace APIWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AuthenticationController : Controller
    {
        private readonly IIdentityService _identityService;
        
        public AuthenticationController(IIdentityService identityService) => _identityService = identityService;

        [Authorize(Roles = Roles.Admin)]
        [SwaggerResponse(200, "Successful operation", Type = typeof(SuccessResponse))]
        [SwaggerResponse(400, "Failed operation", Type = typeof(FailedResponse))]
        [SwaggerOperation(Summary = "", Description = "", Tags = new[] { "AUTHENTICATION" })]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UsuarioCadastroRequest user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                if (user.Role != Roles.Admin) return BadRequest();

                var result = await _identityService.CadastrarUsuario(user);
                if (result.Success) return Ok(result);

                if (result.Errors.Count > 0) return BadRequest(result);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex){ return BadRequest(ex.Message); }
        }

        [SwaggerResponse(200, "Successful operation", Type = typeof(SuccessResponse))]
        [SwaggerResponse(400, "Failed operation", Type = typeof(FailedResponse))]
        [SwaggerOperation(Summary = "", Description = "", Tags = new[] { "AUTHENTICATION" })]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UsuarioLoginRequest user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                var result = await _identityService.LoginUsuario(user);
                if (result.Success) return Ok(result);

                return Unauthorized(result);
            }
            catch (Exception ex){ return BadRequest(ex.Message); }
        }
    }
}
