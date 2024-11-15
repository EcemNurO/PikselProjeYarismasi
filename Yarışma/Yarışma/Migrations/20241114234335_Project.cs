using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yarışma.Migrations
{
    /// <inheritdoc />
    public partial class Project : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Judges_Projects_ProjectId",
                table: "Judges");

            migrationBuilder.DropIndex(
                name: "IX_Judges_ProjectId",
                table: "Judges");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Judges");

            migrationBuilder.AddColumn<int>(
                name: "JudgeId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_JudgeId",
                table: "Projects",
                column: "JudgeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Judges_JudgeId",
                table: "Projects",
                column: "JudgeId",
                principalTable: "Judges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Judges_JudgeId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_JudgeId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "JudgeId",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Judges",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Judges_ProjectId",
                table: "Judges",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Judges_Projects_ProjectId",
                table: "Judges",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
