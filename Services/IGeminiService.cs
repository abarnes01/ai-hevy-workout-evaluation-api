using AiWorkoutPlanAPI.Dtos;

namespace AiWorkoutPlanAPI.Services
{
	public interface IGeminiService
	{
		Task<List<MilestoneDto>> EvaluateMilestones(object promptData);
	}
}
