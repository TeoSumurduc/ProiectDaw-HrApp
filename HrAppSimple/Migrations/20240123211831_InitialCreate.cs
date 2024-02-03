using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrAppSimple.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departament",
                columns: table => new
                {
                    CodDepartament = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departament", x => x.CodDepartament);
                });

            migrationBuilder.CreateTable(
                name: "Proiect",
                columns: table => new
                {
                    CodProiect = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataPredare = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proiect", x => x.CodProiect);
                });

            migrationBuilder.CreateTable(
                name: "Angajat",
                columns: table => new
                {
                    Matricula = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodDepartament = table.Column<int>(type: "int", nullable: false),
                    DepartamentCodDepartament = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Angajat", x => x.Matricula);
                    table.ForeignKey(
                        name: "FK_Angajat_Departament_DepartamentCodDepartament",
                        column: x => x.DepartamentCodDepartament,
                        principalTable: "Departament",
                        principalColumn: "CodDepartament",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locatie",
                columns: table => new
                {
                    CodLocatie = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Oras = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tara = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodDepartament = table.Column<int>(type: "int", nullable: false),
                    DepartamentCodDepartament = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locatie", x => x.CodLocatie);
                    table.ForeignKey(
                        name: "FK_Locatie_Departament_DepartamentCodDepartament",
                        column: x => x.DepartamentCodDepartament,
                        principalTable: "Departament",
                        principalColumn: "CodDepartament",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AngajatProiect",
                columns: table => new
                {
                    Matricula = table.Column<int>(type: "int", nullable: false),
                    CodProiect = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AngajatProiect", x => new { x.Matricula, x.CodProiect });
                    table.ForeignKey(
                        name: "FK_AngajatProiect_Angajat_Matricula",
                        column: x => x.Matricula,
                        principalTable: "Angajat",
                        principalColumn: "Matricula",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AngajatProiect_Proiect_CodProiect",
                        column: x => x.CodProiect,
                        principalTable: "Proiect",
                        principalColumn: "CodProiect",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetaliiAngajat",
                columns: table => new
                {
                    Matricula = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNastere = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAngajare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MatriculaAng = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetaliiAngajat", x => x.Matricula);
                    table.ForeignKey(
                        name: "FK_DetaliiAngajat_Angajat_MatriculaAng",
                        column: x => x.MatriculaAng,
                        principalTable: "Angajat",
                        principalColumn: "Matricula",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Angajat_DepartamentCodDepartament",
                table: "Angajat",
                column: "DepartamentCodDepartament");

            migrationBuilder.CreateIndex(
                name: "IX_AngajatProiect_CodProiect",
                table: "AngajatProiect",
                column: "CodProiect");

            migrationBuilder.CreateIndex(
                name: "IX_DetaliiAngajat_MatriculaAng",
                table: "DetaliiAngajat",
                column: "MatriculaAng",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locatie_DepartamentCodDepartament",
                table: "Locatie",
                column: "DepartamentCodDepartament");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AngajatProiect");

            migrationBuilder.DropTable(
                name: "DetaliiAngajat");

            migrationBuilder.DropTable(
                name: "Locatie");

            migrationBuilder.DropTable(
                name: "Proiect");

            migrationBuilder.DropTable(
                name: "Angajat");

            migrationBuilder.DropTable(
                name: "Departament");
        }
    }
}
