using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yarışma.Migrations
{
    /// <inheritdoc />
    public partial class JudgeSelect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JudgeProfils_univercities_UnivercityId",
                table: "JudgeProfils");

            migrationBuilder.AlterColumn<int>(
                name: "UnivercityId",
                table: "JudgeProfils",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "WorkplaceName",
                table: "JudgeProfils",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JudgeProfils_univercities_UnivercityId",
                table: "JudgeProfils",
                column: "UnivercityId",
                principalTable: "univercities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JudgeProfils_univercities_UnivercityId",
                table: "JudgeProfils");

            migrationBuilder.DropColumn(
                name: "WorkplaceName",
                table: "JudgeProfils");

            migrationBuilder.AlterColumn<int>(
                name: "UnivercityId",
                table: "JudgeProfils",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JudgeProfils_univercities_UnivercityId",
                table: "JudgeProfils",
                column: "UnivercityId",
                principalTable: "univercities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
