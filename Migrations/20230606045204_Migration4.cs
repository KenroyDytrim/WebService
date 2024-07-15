using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web2.Migrations
{
    /// <inheritdoc />
    public partial class Migration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
                //name: "user",
                //columns: table => new
                //{
                    //id = table.Column<int>(type: "integer", nullable: false)
                        //.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    //Login = table.Column<string>(type: "text", nullable: false),
                    //Password = table.Column<string>(type: "text", nullable: false),
                    //Surname = table.Column<string>(type: "text", nullable: false),
                    //Name = table.Column<string>(type: "text", nullable: false),
                    //Phone = table.Column<string>(type: "text", nullable: false),
                    //Email = table.Column<string>(type: "text", nullable: false)
                //},
                //constraints: table =>
                //{
                    //table.PrimaryKey("PK_user", x => x.id);
                //});
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
                //name: "user");
        }
    }
}
