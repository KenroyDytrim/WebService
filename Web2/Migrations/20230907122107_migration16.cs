using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web2.Migrations
{
    /// <inheritdoc />
    public partial class migration16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "patient_analyzes",
                columns: table => new
                {
                    id_patient = table.Column<int>(type: "integer", nullable: false),
                    id_analysis = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patient_analyzes", x => new { x.id_patient, x.id_analysis });
                    table.ForeignKey(
                        name: "FK_patient_analyzes_analyzes_id_analysis",
                        column: x => x.id_analysis,
                        principalTable: "analyzes",
                        principalColumn: "id_analysis",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_patient_analyzes_patient_archive_id_patient",
                        column: x => x.id_patient,
                        principalTable: "patient_archive",
                        principalColumn: "id_patient",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "patient_examinations",
                columns: table => new
                {
                    id_patient = table.Column<int>(type: "integer", nullable: false),
                    id_examination = table.Column<int>(type: "integer", nullable: false),
                    id_analysis = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patient_examinations", x => new { x.id_patient, x.id_examination });
                    table.ForeignKey(
                        name: "FK_patient_examinations_examination_id_analysis",
                        column: x => x.id_analysis,
                        principalTable: "examination",
                        principalColumn: "id_examination");
                    table.ForeignKey(
                        name: "FK_patient_examinations_patient_archive_id_patient",
                        column: x => x.id_patient,
                        principalTable: "patient_archive",
                        principalColumn: "id_patient",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_patients",
                columns: table => new
                {
                    id_user = table.Column<string>(type: "text", nullable: false),
                    id_patient = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_patients", x => new { x.id_user, x.id_patient });
                    table.ForeignKey(
                        name: "FK_user_patients_AspNetUsers_id_user",
                        column: x => x.id_user,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_patients_patient_archive_id_patient",
                        column: x => x.id_patient,
                        principalTable: "patient_archive",
                        principalColumn: "id_patient",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_patient_analyzes_id_analysis",
                table: "patient_analyzes",
                column: "id_analysis");

            migrationBuilder.CreateIndex(
                name: "IX_patient_examinations_id_analysis",
                table: "patient_examinations",
                column: "id_analysis");

            migrationBuilder.CreateIndex(
                name: "IX_user_patients_id_patient",
                table: "user_patients",
                column: "id_patient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "patient_analyzes");

            migrationBuilder.DropTable(
                name: "patient_examinations");

            migrationBuilder.DropTable(
                name: "user_patients");
        }
    }
}
