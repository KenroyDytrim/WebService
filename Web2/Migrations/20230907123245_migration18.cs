using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web2.Migrations
{
    /// <inheritdoc />
    public partial class migration18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patient_examinations_examination_id_analysis",
                table: "patient_examinations");

            migrationBuilder.DropIndex(
                name: "IX_patient_examinations_id_analysis",
                table: "patient_examinations");

            migrationBuilder.DropColumn(
                name: "id_analysis",
                table: "patient_examinations");

            migrationBuilder.CreateIndex(
                name: "IX_patient_examinations_id_examination",
                table: "patient_examinations",
                column: "id_examination");

            migrationBuilder.AddForeignKey(
                name: "FK_patient_examinations_examination_id_examination",
                table: "patient_examinations",
                column: "id_examination",
                principalTable: "examination",
                principalColumn: "id_examination",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patient_examinations_examination_id_examination",
                table: "patient_examinations");

            migrationBuilder.DropIndex(
                name: "IX_patient_examinations_id_examination",
                table: "patient_examinations");

            migrationBuilder.AddColumn<int>(
                name: "id_analysis",
                table: "patient_examinations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_patient_examinations_id_analysis",
                table: "patient_examinations",
                column: "id_analysis");

            migrationBuilder.AddForeignKey(
                name: "FK_patient_examinations_examination_id_analysis",
                table: "patient_examinations",
                column: "id_analysis",
                principalTable: "examination",
                principalColumn: "id_examination");
        }
    }
}
