using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace WebAppEF.AdventureS.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleGuid",
                schema: "mad",
                table: "Role",
                type: "nvarchar(max)",
                nullable: false,
                defaultValueSql: "NEWID()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleGuid",
                schema: "mad",
                table: "Role");
        }
    }
}
