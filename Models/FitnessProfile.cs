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

		public float? Weight { get; set; }      // kg
		public float? Height { get; set; }      // cm
		public int? Age { get; set; }
		public string? Gender { get; set; }     // Male/Female/Other
		public float? BodyMusclePercentage { get; set; } // %
		public int? WorkoutDaysPerWeek { get; set; }

		// Relationship to User
		[Required]
		public int UserId { get; set; }
		public User User { get; set; } = null!;
	}
}
