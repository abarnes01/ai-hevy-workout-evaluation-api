using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiWorkoutPlanAPI.Migrations
{
	public partial class AddFitnessProfileAndHevyKey : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "WorkoutPlans");

			migrationBuilder.CreateTable(
				name: "FitnessProfiles",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Goal = table.Column<string>(type: "TEXT", nullable: false),
					Weight = table.Column<float>(type: "REAL", nullable: true),
					Height = table.Column<float>(type: "REAL", nullable: true),
					Age = table.Column<int>(type: "INTEGER", nullable: true),
					Gender = table.Column<string>(type: "TEXT", nullable: true),
					BodyMusclePercentage = table.Column<float>(type: "REAL", nullable: true),
					WorkoutDaysPerWeek = table.Column<int>(type: "INTEGER", nullable: true),
					UserId = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_FitnessProfiles", x => x.Id);
					table.ForeignKey(
						name: "FK_FitnessProfiles_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_FitnessProfiles_UserId",
				table: "FitnessProfiles",
				column: "UserId",
				unique: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "FitnessProfiles");

			migrationBuilder.CreateTable(
				name: "WorkoutPlans",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UserId = table.Column<int>(type: "INTEGER", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
					Goal = table.Column<string>(type: "TEXT", nullable: false),
					HevyPayload = table.Column<string>(type: "TEXT", nullable: true),
					Plan = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_WorkoutPlans", x => x.Id);
					table.ForeignKey(
						name: "FK_WorkoutPlans_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_WorkoutPlans_UserId",
				table: "WorkoutPlans",
				column: "UserId");
		}
	}
}
