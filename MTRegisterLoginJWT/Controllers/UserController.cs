using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTRegisterLoginJWT.Data.DTO;
using MTRegisterLoginJWT.Data;
using MTRegisterLoginJWT.Data.Models;
using MTRegisterLoginJWT.Interfaces;

namespace MTRegisterLoginJWT.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<ICollection<User>>>> GetUsersAsync()
		{
			var result = await _userService.GetUsersAsync();
			if (!result.Success)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
	}
}
