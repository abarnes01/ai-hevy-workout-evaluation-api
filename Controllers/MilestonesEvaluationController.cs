using System.Security.Claims;
using AiWorkoutPlanAPI.Data;
using AiWorkoutPlanAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiWorkoutPlanAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class MilestonesEvaluationController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IHevyService _hevyService;
		private readonly IGeminiService _geminiService;

		public MilestonesEvaluationController(
			AppDbContext context,
			IHevyService hevyService,
			IGeminiService geminiService)
		{
			_context = context;
			_hevyService = hevyService;
			_geminiService = geminiService;
		}

		[HttpGet("evaluate")]
		public async Task<IActionResult> EvaluateMilestones()
		{
			var userId = GetUserId();
			if (userId == null) return Unauthorized(new { Error = "Invalid user context." });

			var user = await _context.Users
				.Include(u => u.FitnessProfile)
				.FirstOrDefaultAsync(u => u.Id == userId.Value);

			if (user == null) return NotFound(new { Error = "User not found." });
			if (user.FitnessProfile == null || string.IsNullOrWhiteSpace(user.FitnessProfile.Goal))
				return BadRequest(new { Error = "Fitness goal not set." });
			if (string.IsNullOrEmpty(user.HevyApiKey))
				return BadRequest(new { Error = "Hevy API key not set." });

			var routinesJson = await _hevyService.GetRoutinesAsync(user.HevyApiKey, page: 1, pageSize: 10);

			var promptData = new
			{
				FitnessProfile = new
				{
					user.FitnessProfile.Goal,
					user.FitnessProfile.Weight,
					user.FitnessProfile.Height,
					user.FitnessProfile.Age,
					user.FitnessProfile.Gender,
					user.FitnessProfile.BodyMusclePercentage,
					user.FitnessProfile.WorkoutDaysPerWeek
				},
				Routines = routinesJson
			};

			var milestones = await _geminiService.EvaluateMilestones(promptData);

			return Ok(new
			{
				FitnessProfile = promptData.FitnessProfile,
				Routines = routinesJson,
				SuggestedMilestones = milestones
			});
		}

		private int? GetUserId()
		{
			var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (int.TryParse(userIdString, out int userId)) return userId;
			return null;
		}
	}
}
