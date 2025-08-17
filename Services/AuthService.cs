using Microsoft.EntityFrameworkCore;
using AiWorkoutPlanAPI.Data;
using AiWorkoutPlanAPI.Helpers;

namespace AiWorkoutPlanAPI.Services
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _context;
		private readonly IConfiguration _config;

		public AuthService(AppDbContext context, IConfiguration config)
		{
			_context = context;
			_config = config;
		}

		public async Task<(bool Success, string Token, string Message)> RegisterAsync(string username, string password)
		{
			if (await _context.Users.AnyAsync(u => u.Username == username.ToLower()))
				return (false, "", "User already exists");

			AuthHelpers.CreatePasswordHash(password, out byte[] hash, out byte[] salt);

			var user = new User
			{
				Username = username.ToLower(),
				PasswordHash = hash,
				PasswordSalt = salt
			};

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			var token = AuthHelpers.CreateToken(user, _config);
			return (true, token, "Registration successful");
		}

		public async Task<(bool Success, string Token, string Message)> LoginAsync(string username, string password)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username.ToLower());
			if (user == null)
				return (false, "", "User not found");

			if (!AuthHelpers.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
				return (false, "", "Incorrect password");

			var token = AuthHelpers.CreateToken(user, _config);
			return (true, token, "Login successful");
		}
	}
}
