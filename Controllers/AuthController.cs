using Microsoft.AspNetCore.Mvc;
using AiWorkoutPlanAPI.DTOs;
using AiWorkoutPlanAPI.Services;

namespace AiWorkoutPlanAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(UserRegisterDto request)
		{
			var result = await _authService.RegisterAsync(request.Email, request.Password);
			if (!result.Success)
				return BadRequest(result.Message);

			return Ok(result.Token);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(UserLoginDto request)
		{
			var result = await _authService.LoginAsync(request.Email, request.Password);
			if (!result.Success)
				return BadRequest(result.Message);

			return Ok(result.Token);
		}
	}
}
