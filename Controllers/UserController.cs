using System.Security.Claims;
using AiWorkoutPlanAPI.Dtos;
using AiWorkoutPlanAPI.Models;
using AiWorkoutPlanAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AiWorkoutPlanAPI.DTOs;

namespace AiWorkoutPlanAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class UserController : ControllerBase
	{
		private readonly AppDbContext _context;

		public UserController(AppDbContext context)
		{
			_context = context;
		}

		[HttpPost("hevy-key")]
		public async Task<IActionResult> SetHevyApiKey([FromBody] SetHevyApiKeyDto dto)
		{
			var user = await GetCurrentUserAsync();
			if (user == null) return NotFound(new { Error = "User not found." });

			user.HevyApiKey = dto.ApiKey;
			await _context.SaveChangesAsync();

			return Ok(new { Message = "Hevy API key saved successfully." });
		}

		[HttpGet("hevy-key")]
		public async Task<IActionResult> GetHevyApiKey()
		{
			var user = await GetCurrentUserAsync();
			if (user == null) return NotFound(new { Error = "User not found." });

			return Ok(new
			{
				HasApiKey = !string.IsNullOrEmpty(user.HevyApiKey),
			});
		}

		[HttpGet("profile")]
		public async Task<IActionResult> GetFitnessProfile()
		{
			var user = await GetCurrentUserAsync();
			if (user == null) return NotFound(new { Error = "User not found." });

			if (user.FitnessProfile == null)
				return Ok(new FitnessProfileDto());

			var dto = new FitnessProfileDto
			{
				Goal = user.FitnessProfile.Goal,
				Weight = user.FitnessProfile.Weight,
				Height = user.FitnessProfile.Height,
				Age = user.FitnessProfile.Age,
				Gender = user.FitnessProfile.Gender,
				BodyMusclePercentage = user.FitnessProfile.BodyMusclePercentage,
				WorkoutDaysPerWeek = user.FitnessProfile.WorkoutDaysPerWeek
			};

			return Ok(dto);
		}

		[HttpPost("profile")]
		public async Task<IActionResult> SetFitnessProfile([FromBody] FitnessProfileDto dto)
		{
			if (string.IsNullOrWhiteSpace(dto.Goal))
				return BadRequest(new { Error = "Goal is required." });

			var user = await GetCurrentUserAsync();
			if (user == null) return NotFound(new { Error = "User not found." });

			if (user.FitnessProfile == null)
				user.FitnessProfile = new FitnessProfile();

			user.FitnessProfile.Goal = dto.Goal;
			user.FitnessProfile.Weight = dto.Weight;
			user.FitnessProfile.Height = dto.Height;
			user.FitnessProfile.Age = dto.Age;
			user.FitnessProfile.Gender = dto.Gender;
			user.FitnessProfile.BodyMusclePercentage = dto.BodyMusclePercentage;
			user.FitnessProfile.WorkoutDaysPerWeek = dto.WorkoutDaysPerWeek;

			await _context.SaveChangesAsync();
			return Ok(new { Message = "Fitness profile saved successfully." });
		}

		private async Task<User?> GetCurrentUserAsync()
		{
			var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
				return null;

			return await _context.Users
				.Include(u => u.FitnessProfile)
				.FirstOrDefaultAsync(u => u.Id == userId);
		}
	}
}
