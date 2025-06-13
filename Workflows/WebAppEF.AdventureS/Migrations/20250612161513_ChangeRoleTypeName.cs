using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppEF.AdventureS.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRoleTypeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "mad",
                table: "Role",
                newName: "RoleType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoleType",
                schema: "mad",
                table: "Role",
                newName: "Name");
        }
    }
}
