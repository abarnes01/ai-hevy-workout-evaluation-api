namespace AiWorkoutPlanAPI.Services
{
	public interface IHevyService
	{
		Task<string> GetRoutinesAsync(string apiKey, int page = 1, int pageSize = 10);
	}
}
