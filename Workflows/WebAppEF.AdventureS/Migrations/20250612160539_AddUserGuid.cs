using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppEF.AdventureS.Migrations
{
    /// <inheritdoc />
    public partial class AddUserGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserGuid",
                schema: "mad",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGuid",
                schema: "mad",
                table: "User");
        }
    }
}
