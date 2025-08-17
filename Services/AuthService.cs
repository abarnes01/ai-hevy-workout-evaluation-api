using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AiWorkoutPlanAPI.Models;
using AiWorkoutPlanAPI.Data;

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

			CreatePasswordHash(password, out byte[] hash, out byte[] salt);

			var user = new User
			{
				Username = username.ToLower(),
				PasswordHash = hash,
				PasswordSalt = salt
			};

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			var token = CreateToken(user);
			return (true, token, "Registration successful");
		}

		public async Task<(bool Success, string Token, string Message)> LoginAsync(string username, string password)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username.ToLower());
			if (user == null)
				return (false, "", "User not found");

			if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
				return (false, "", "Incorrect password");

			var token = CreateToken(user);
			return (true, token, "Login successful");
		}

		// Helpers

		private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
		{
			using var hmac = new HMACSHA512();
			salt = hmac.Key;
			hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
		}

		private bool VerifyPassword(string password, byte[] hash, byte[] salt)
		{
			using var hmac = new HMACSHA512(salt);
			var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			return computedHash.SequenceEqual(hash);
		}

		private string CreateToken(User user)
		{
			var claims = new List<Claim> {
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.Username) // changed from Email to Username
            };

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(7),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
