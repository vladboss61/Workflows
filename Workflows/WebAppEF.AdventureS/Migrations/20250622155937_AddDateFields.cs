using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppEF.AdventureS.Migrations
{
    /// <inheritdoc />
    public partial class AddDateFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "mad",
                table: "RoleTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "mad",
                table: "RoleTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "mad",
                table: "RoleTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { DateTime.UtcNow, null });

            migrationBuilder.UpdateData(
                schema: "mad",
                table: "RoleTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { DateTime.UtcNow, null });

            migrationBuilder.UpdateData(
                schema: "mad",
                table: "RoleTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { DateTime.UtcNow, null });

            migrationBuilder.UpdateData(
                schema: "mad",
                table: "RoleTypes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { DateTime.UtcNow, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "mad",
                table: "RoleTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "mad",
                table: "RoleTypes");
        }
    }
}
