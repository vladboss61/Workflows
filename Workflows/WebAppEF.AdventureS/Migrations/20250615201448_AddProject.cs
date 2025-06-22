using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppEF.AdventureS.Migrations
{
    /// <inheritdoc />
    public partial class AddProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                schema: "mad",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Project",
                schema: "mad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_ProjectId",
                schema: "mad",
                table: "User",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Project_ProjectId",
                schema: "mad",
                table: "User",
                column: "ProjectId",
                principalSchema: "mad",
                principalTable: "Project",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Project_ProjectId",
                schema: "mad",
                table: "User");

            migrationBuilder.DropTable(
                name: "Project",
                schema: "mad");

            migrationBuilder.DropIndex(
                name: "IX_User_ProjectId",
                schema: "mad",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                schema: "mad",
                table: "User");
        }
    }
}
