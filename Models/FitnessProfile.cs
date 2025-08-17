using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiWorkoutPlanAPI.Models
{
	public class FitnessProfile
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Goal { get; set; } = string.Empty;

		public float? Weight { get; set; }
		public float? Height { get; set; }
		public int? Age { get; set; }
		public string? Gender { get; set; }
		public float? BodyMusclePercentage { get; set; }
		public int? WorkoutDaysPerWeek { get; set; }

		[Required]
		public int UserId { get; set; }
		public User User { get; set; } = null!;
	}
}
