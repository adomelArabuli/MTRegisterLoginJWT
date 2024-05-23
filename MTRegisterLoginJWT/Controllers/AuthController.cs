using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTRegisterLoginJWT.Data;
using MTRegisterLoginJWT.Data.DTO;
using MTRegisterLoginJWT.Interfaces;

namespace MTRegisterLoginJWT.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("Register")]
		public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDTO model)
		{
			var result = await _authService.Register(model);
			if(!result.Success)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		[HttpPost("Login")]
		public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDTO model)
		{
			var result = await _authService.Login(model);
			if (!result.Success)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
	}
}
