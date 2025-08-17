namespace AiWorkoutPlanAPI.Services
{
	public interface IAuthService
	{
		Task<(bool Success, string Token, string Message)> RegisterAsync(string email, string password);
		Task<(bool Success, string Token, string Message)> LoginAsync(string email, string password);
	}
}
