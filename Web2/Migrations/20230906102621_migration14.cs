using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web2.Migrations
{
    /// <inheritdoc />
    public partial class migration14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patient_archive_patient_groups_id_group",
                table: "patient_archive");

            migrationBuilder.DropIndex(
                name: "IX_patient_archive_id_group",
                table: "patient_archive");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "archive_group",
                columns: table => new
                {
                    id_patient = table.Column<int>(type: "integer", nullable: false),
                    id_group = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_archive_group", x => new { x.id_patient, x.id_group });
                    table.ForeignKey(
                        name: "FK_archive_group_patient_archive_id_patient",
                        column: x => x.id_patient,
                        principalTable: "patient_archive",
                        principalColumn: "id_patient",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_archive_group_patient_groups_id_group",
                        column: x => x.id_group,
                        principalTable: "patient_groups",
                        principalColumn: "id_group",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_archive_group_id_group",
                table: "archive_group",
                column: "id_group");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "archive_group");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_patient_archive_id_group",
                table: "patient_archive",
                column: "id_group");

            migrationBuilder.AddForeignKey(
                name: "FK_patient_archive_patient_groups_id_group",
                table: "patient_archive",
                column: "id_group",
                principalTable: "patient_groups",
                principalColumn: "id_group",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
