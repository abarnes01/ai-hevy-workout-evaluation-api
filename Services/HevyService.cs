using System.Net.Http.Headers;

namespace AiWorkoutPlanAPI.Services
{
	public class HevyService : IHevyService
	{
		private readonly HttpClient _httpClient;

		public HevyService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://api.hevyapp.com/v1/");
		}

		public async Task<string> GetRoutinesAsync(string apiKey, int page = 1, int pageSize = 10)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, $"routines?page={page}&pageSize={pageSize}");
			request.Headers.Add("api-key", apiKey);

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();
		}
	}
}
