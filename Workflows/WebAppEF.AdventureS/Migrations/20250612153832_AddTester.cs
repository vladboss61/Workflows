using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppEF.AdventureS.Migrations
{
    /// <inheritdoc />
    public partial class AddTester : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "mad",
                table: "RoleTypes",
                columns: new[] { "Id", "RoleDescription", "RoleName", "RoleType" },
                values: new object[] { 4, "Tester with acces to some internal resources.", "Tester", 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "mad",
                table: "RoleTypes",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
