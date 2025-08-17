namespace AiWorkoutPlanAPI.Dtos
{
	public class FitnessProfileDto
	{
		public string Goal { get; set; } = string.Empty;
		public float? Weight { get; set; }
		public float? Height { get; set; }
		public int? Age { get; set; }
		public string? Gender { get; set; }
		public float? BodyMusclePercentage { get; set; }
		public int? WorkoutDaysPerWeek { get; set; }
	}
}
