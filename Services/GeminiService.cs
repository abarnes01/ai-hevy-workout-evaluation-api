using System.Text.Json;
using GenerativeAI;

namespace AiWorkoutPlanAPI.Services
{
	public class GeminiService : IGeminiService
	{
		private readonly GenerativeModel _model;

		public GeminiService(string apiKey)
		{
			var googleAI = new GoogleAi(apiKey);
			_model = googleAI.CreateGenerativeModel("models/gemini-1.5-flash");
		}

		public async Task<List<MilestoneDto>> EvaluateMilestones(object promptData)
		{
			var jsonOptions = new JsonSerializerOptions
			{
				ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
				WriteIndented = true
			};
			var serializedData = JsonSerializer.Serialize(promptData, jsonOptions);

			var prompt = $@"
You are a fitness coach. A user has the following data:
{serializedData}

Please generate a list of realistic milestones and exercises they should prioritize.
Return only a JSON array like this:
[
  {{
    ""exercise"": ""<exercise name>"",
    ""focus"": ""<what to improve>"",
    ""milestone"": ""<realistic target>""
  }}
]";

			var response = await _model.GenerateContentAsync(prompt);

			var rawText = response.Text().Trim();
			rawText = rawText.Trim('`');

			int jsonStart = rawText.IndexOfAny(new char[] { '[', '{' });
			if (jsonStart >= 0)
				rawText = rawText[jsonStart..].Trim();

			try
			{
				var milestones = JsonSerializer.Deserialize<List<MilestoneDto>>(rawText,
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				return milestones ?? new List<MilestoneDto>();
			}
			catch (JsonException ex)
			{
				Console.WriteLine("JSON deserialization error: " + ex.Message);
				return new List<MilestoneDto>();
			}
		}
	}
}
