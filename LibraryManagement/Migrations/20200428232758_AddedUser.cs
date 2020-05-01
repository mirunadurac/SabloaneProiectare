using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryManagement.Migrations
{
    public partial class AddedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Students_Gender_Enum_Constraint",
                table: "Students");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Students_Role_Enum_Constraint",
                table: "Students");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "Users");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_Users_Gender_Enum_Constraint",
                table: "Users",
                sql: "[Gender] IN(0, 1, 2)");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_Users_Role_Enum_Constraint",
                table: "Users",
                sql: "[Role] IN(0, 1)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Gender_Enum_Constraint",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Role_Enum_Constraint",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Students");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_Students_Gender_Enum_Constraint",
                table: "Students",
                sql: "[Gender] IN(0, 1, 2)");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_Students_Role_Enum_Constraint",
                table: "Students",
                sql: "[Role] IN(0, 1)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");
        }
    }
}
