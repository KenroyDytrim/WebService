using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web2.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "patient_groups",
                columns: table => new
                {
                    id_group = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patient_groups", x => x.id_group);
                });

            migrationBuilder.CreateTable(
                name: "patient_archive",
                columns: table => new
                {
                    id_patient = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_group = table.Column<int>(type: "integer", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    patronymic = table.Column<string>(type: "text", nullable: true),
                    birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    phone_num = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patient_archive", x => x.id_patient);
                    table.ForeignKey(
                        name: "FK_patient_archive_patient_groups_id_group",
                        column: x => x.id_group,
                        principalTable: "patient_groups",
                        principalColumn: "id_group",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "analyzes",
                columns: table => new
                {
                    id_analysis = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_patient = table.Column<int>(type: "integer", nullable: false),
                    serum_calcium = table.Column<double>(type: "double precision", nullable: false),
                    serum_phosphorus = table.Column<double>(type: "double precision", nullable: false),
                    serum_oxyproline = table.Column<double>(type: "double precision", nullable: false),
                    calcium_excretion = table.Column<double>(type: "double precision", nullable: false),
                    phosphorus_excretion = table.Column<double>(type: "double precision", nullable: false),
                    oxyproline_excretion = table.Column<double>(type: "double precision", nullable: false),
                    severity_of_dst = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_analyzes", x => x.id_analysis);
                    table.ForeignKey(
                        name: "FK_analyzes_patient_archive_id_patient",
                        column: x => x.id_patient,
                        principalTable: "patient_archive",
                        principalColumn: "id_patient",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "examination",
                columns: table => new
                {
                    id_examination = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_patient = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    posture_holding_time = table.Column<double>(type: "double precision", nullable: false),
                    the_amount_of_kyphosis_in_degress = table.Column<double>(type: "double precision", nullable: false),
                    changing_the_contours_of_the_end_plates = table.Column<bool>(type: "boolean", nullable: false),
                    wedgeshaped_vertebral_bodies = table.Column<bool>(type: "boolean", nullable: false),
                    schmorl_hernia = table.Column<bool>(type: "boolean", nullable: false),
                    osteoporosis_of_the_vertebrae = table.Column<bool>(type: "boolean", nullable: false),
                    decrease_in_the_height_of_the_intervertebral_disc = table.Column<bool>(type: "boolean", nullable: false),
                    change_in_the_contours_of_the_apophyses = table.Column<bool>(type: "boolean", nullable: false),
                    signs_of_osteochondrosis = table.Column<bool>(type: "boolean", nullable: false),
                    stabilographic_changes = table.Column<bool>(type: "boolean", nullable: false),
                    enmg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_examination", x => x.id_examination);
                    table.ForeignKey(
                        name: "FK_examination_patient_archive_id_patient",
                        column: x => x.id_patient,
                        principalTable: "patient_archive",
                        principalColumn: "id_patient",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_analyzes_id_patient",
                table: "analyzes",
                column: "id_patient");

            migrationBuilder.CreateIndex(
                name: "IX_examination_id_patient",
                table: "examination",
                column: "id_patient");

            migrationBuilder.CreateIndex(
                name: "IX_patient_archive_id_group",
                table: "patient_archive",
                column: "id_group");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "analyzes");

            migrationBuilder.DropTable(
                name: "examination");

            migrationBuilder.DropTable(
                name: "patient_archive");

            migrationBuilder.DropTable(
                name: "patient_groups");
        }
    }
}
