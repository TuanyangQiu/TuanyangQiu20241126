using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Handler.Model;
using Handler.Service;

namespace myapi.Controllers
{

	[ApiController]
	[Route("api/[controller]")]

	public class UserController : ControllerBase
	{

		private readonly ILogger<UserController> _logger;
		private readonly IUserService _userSvc;
		public UserController(ILogger<UserController> logger, IUserService userSvc)
		{
			_logger = logger;
			_userSvc = userSvc;
		}

		[HttpPost]
		public async Task<IActionResult> CreateNewUser([FromBody] User user)
		{
			string ret = await _userSvc.SaveUser(user);
			if(string.IsNullOrEmpty(ret))
				return Ok();
			else
				return BadRequest(new ApiErrorDto() { Message=ret} );	
 
		}
	}
}
