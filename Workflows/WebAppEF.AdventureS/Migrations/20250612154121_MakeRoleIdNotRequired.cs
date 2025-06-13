using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppEF.AdventureS.Migrations
{
    /// <inheritdoc />
    public partial class MakeRoleIdNotRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                schema: "mad",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                schema: "mad",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                schema: "mad",
                table: "User",
                column: "RoleId",
                principalSchema: "mad",
                principalTable: "Role",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                schema: "mad",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                schema: "mad",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                schema: "mad",
                table: "User",
                column: "RoleId",
                principalSchema: "mad",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
