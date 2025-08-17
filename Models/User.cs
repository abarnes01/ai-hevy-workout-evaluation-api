using AiWorkoutPlanAPI.Models;

public class User
{
	public int Id { get; set; }
	public string Username { get; set; } = string.Empty;
	public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
	public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
	public string? HevyApiKey { get; set; }

	public FitnessProfile? FitnessProfile { get; set; }  // navigation property
}
