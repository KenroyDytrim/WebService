using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web2.Migrations
{
    /// <inheritdoc />
    public partial class Migration11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "AspNetUsers");

            //migrationBuilder.CreateTable(
                //name: "user",
                //columns: table => new
                //{
                    //Id = table.Column<string>(type: "text", nullable: false),
                    //Login = table.Column<string>(type: "text", nullable: false),
                    //Password = table.Column<string>(type: "text", nullable: false),
                    //Surname = table.Column<string>(type: "text", nullable: false),
                    //Name = table.Column<string>(type: "text", nullable: false),
                    //Phone = table.Column<string>(type: "text", nullable: false),
                    //Email = table.Column<string>(type: "text", nullable: false),
                    //UserName = table.Column<string>(type: "text", nullable: true),
                    //NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    //NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    //EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    //PasswordHash = table.Column<string>(type: "text", nullable: true),
                    //SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    //ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    //PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    //PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    //TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    //LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    //LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    //AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                //},
                //constraints: table =>
                //{
                    //table.PrimaryKey("PK_user", x => x.Id);
                //});
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
                //name: "user");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
