using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web2.Migrations
{
    /// <inheritdoc />
    public partial class migration15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id_group",
                table: "patient_archive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_group",
                table: "patient_archive",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
