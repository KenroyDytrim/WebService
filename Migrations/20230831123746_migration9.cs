using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web2.Migrations
{
    /// <inheritdoc />
    public partial class migration9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_idRole",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_idRole",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "idRole",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "idRole",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_idRole",
                table: "AspNetUsers",
                column: "idRole");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_idRole",
                table: "AspNetUsers",
                column: "idRole",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }
    }
}
