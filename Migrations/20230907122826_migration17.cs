using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web2.Migrations
{
    /// <inheritdoc />
    public partial class migration17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_analyzes_patient_archive_id_patient",
                table: "analyzes");

            migrationBuilder.DropForeignKey(
                name: "FK_examination_patient_archive_id_patient",
                table: "examination");

            migrationBuilder.DropIndex(
                name: "IX_examination_id_patient",
                table: "examination");

            migrationBuilder.DropIndex(
                name: "IX_analyzes_id_patient",
                table: "analyzes");

            migrationBuilder.DropColumn(
                name: "id_patient",
                table: "examination");

            migrationBuilder.DropColumn(
                name: "id_patient",
                table: "analyzes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_patient",
                table: "examination",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id_patient",
                table: "analyzes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_examination_id_patient",
                table: "examination",
                column: "id_patient");

            migrationBuilder.CreateIndex(
                name: "IX_analyzes_id_patient",
                table: "analyzes",
                column: "id_patient");

            migrationBuilder.AddForeignKey(
                name: "FK_analyzes_patient_archive_id_patient",
                table: "analyzes",
                column: "id_patient",
                principalTable: "patient_archive",
                principalColumn: "id_patient",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_examination_patient_archive_id_patient",
                table: "examination",
                column: "id_patient",
                principalTable: "patient_archive",
                principalColumn: "id_patient",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
