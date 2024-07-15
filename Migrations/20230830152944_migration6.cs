using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web2.Migrations
{
    /// <inheritdoc />
    public partial class migration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
                //name: "idRole",
                //table: "user",
                //type: "integer",
                //nullable: false,
                //defaultValue: 0);

            //migrationBuilder.CreateTable(
                //name: "role",
                //columns: table => new
                //{
                    //idRole = table.Column<int>(type: "integer", nullable: false)
                        //.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    //Name = table.Column<string>(type: "text", nullable: false)
                //},
                //constraints: table =>
                //{
                    //table.PrimaryKey("PK_role", x => x.idRole);
                //});

            //migrationBuilder.CreateIndex(
                //name: "IX_user_idRole",
                //table: "user",
                //column: "idRole");

            //migrationBuilder.AddForeignKey(
                //name: "FK_user_role_idRole",
                //table: "user",
                //column: "idRole",
                //principalTable: "role",
                //principalColumn: "idRole",
                //onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
                //name: "FK_user_role_idRole",
                //table: "user");

            //migrationBuilder.DropTable(
                //name: "role");

            //migrationBuilder.DropIndex(
                //name: "IX_user_idRole",
                //table: "user");

            //migrationBuilder.DropColumn(
                //name: "idRole",
                //table: "user");
        }
    }
}
