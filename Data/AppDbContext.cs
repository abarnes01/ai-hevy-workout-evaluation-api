using Microsoft.EntityFrameworkCore;
using AiWorkoutPlanAPI.Models;

namespace AiWorkoutPlanAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
			//stub
		}

		public DbSet<User> Users { get; set; } = null!;
		public DbSet<FitnessProfile> FitnessProfiles { get; set; } = null!;
	}
}
