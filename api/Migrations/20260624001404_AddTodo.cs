using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "todo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataLimite = table.Column<DateOnly>(type: "date", nullable: false),
                    PlantioId = table.Column<int>(type: "int", nullable: true),
                    HortaId = table.Column<int>(type: "int", nullable: false),
                    MembroId = table.Column<int>(type: "int", nullable: true),
                    CompletedAt = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CURRENT_DATE)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_todo_horta_HortaId",
                        column: x => x.HortaId,
                        principalTable: "horta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_todo_membro_MembroId",
                        column: x => x.MembroId,
                        principalTable: "membro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_todo_plantio_PlantioId",
                        column: x => x.PlantioId,
                        principalTable: "plantio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_todo_HortaId",
                table: "todo",
                column: "HortaId");

            migrationBuilder.CreateIndex(
                name: "IX_todo_MembroId",
                table: "todo",
                column: "MembroId");

            migrationBuilder.CreateIndex(
                name: "IX_todo_PlantioId",
                table: "todo",
                column: "PlantioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "todo");
        }
    }
}
