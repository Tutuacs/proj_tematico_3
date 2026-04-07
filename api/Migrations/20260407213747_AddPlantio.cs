using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddPlantio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "plantio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EspecieId = table.Column<int>(type: "int", nullable: false),
                    HortaId = table.Column<int>(type: "int", nullable: false),
                    DataPlantio = table.Column<DateOnly>(type: "date", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CURRENT_DATE)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plantio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_plantio_especie_EspecieId",
                        column: x => x.EspecieId,
                        principalTable: "especie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_plantio_horta_HortaId",
                        column: x => x.HortaId,
                        principalTable: "horta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_plantio_EspecieId",
                table: "plantio",
                column: "EspecieId");

            migrationBuilder.CreateIndex(
                name: "IX_plantio_HortaId",
                table: "plantio",
                column: "HortaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "plantio");
        }
    }
}
