using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiWorkoutPlanAPI.Migrations
{
	public partial class InitialMigration : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Username = table.Column<string>(type: "TEXT", nullable: false),
					PasswordHash = table.Column<byte[]>(type: "BLOB", nullable: false),
					PasswordSalt = table.Column<byte[]>(type: "BLOB", nullable: false),
					HevyApiKey = table.Column<string>(type: "TEXT", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "WorkoutPlans",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UserId = table.Column<int>(type: "INTEGER", nullable: false),
					Goal = table.Column<string>(type: "TEXT", nullable: false),
					Plan = table.Column<string>(type: "TEXT", nullable: false),
					HevyPayload = table.Column<string>(type: "TEXT", nullable: true),
					CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "WorkoutPlans");

			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}
