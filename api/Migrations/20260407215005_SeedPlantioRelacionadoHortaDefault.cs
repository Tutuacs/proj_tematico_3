using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedPlantioRelacionadoHortaDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                        migrationBuilder.Sql(@"
                                INSERT INTO plantio (EspecieId, HortaId, DataPlantio, Quantidade, Status, CreatedAt)
                                SELECT e.Id, h.Id, CURRENT_DATE(), 20, 0, CURRENT_DATE()
                                FROM especie e
                                INNER JOIN horta h ON h.Nome = 'Horta Comunitaria Central' AND h.Local = 'Rua das Flores, 120 - Centro'
                                WHERE e.Nome = 'Alface'
                                    AND NOT EXISTS (
                                            SELECT 1 FROM plantio p
                                            WHERE p.EspecieId = e.Id AND p.HortaId = h.Id
                                    );

                                INSERT INTO plantio (EspecieId, HortaId, DataPlantio, Quantidade, Status, CreatedAt)
                                SELECT e.Id, h.Id, DATE_SUB(CURRENT_DATE(), INTERVAL 7 DAY), 12, 0, CURRENT_DATE()
                                FROM especie e
                                INNER JOIN horta h ON h.Nome = 'Horta Comunitaria Central' AND h.Local = 'Rua das Flores, 120 - Centro'
                                WHERE e.Nome = 'Tomate'
                                    AND NOT EXISTS (
                                            SELECT 1 FROM plantio p
                                            WHERE p.EspecieId = e.Id AND p.HortaId = h.Id
                                    );

                                INSERT INTO plantio (EspecieId, HortaId, DataPlantio, Quantidade, Status, CreatedAt)
                                SELECT e.Id, h.Id, DATE_SUB(CURRENT_DATE(), INTERVAL 15 DAY), 30, 0, CURRENT_DATE()
                                FROM especie e
                                INNER JOIN horta h ON h.Nome = 'Horta Comunitaria Central' AND h.Local = 'Rua das Flores, 120 - Centro'
                                WHERE e.Nome = 'Cenoura'
                                    AND NOT EXISTS (
                                            SELECT 1 FROM plantio p
                                            WHERE p.EspecieId = e.Id AND p.HortaId = h.Id
                                    );

                                INSERT INTO plantio (EspecieId, HortaId, DataPlantio, Quantidade, Status, CreatedAt)
                                SELECT e.Id, h.Id, DATE_SUB(CURRENT_DATE(), INTERVAL 3 DAY), 18, 0, CURRENT_DATE()
                                FROM especie e
                                INNER JOIN horta h ON h.Nome = 'Horta Comunitaria Central' AND h.Local = 'Rua das Flores, 120 - Centro'
                                WHERE e.Nome = 'Cebolinha'
                                    AND NOT EXISTS (
                                            SELECT 1 FROM plantio p
                                            WHERE p.EspecieId = e.Id AND p.HortaId = h.Id
                                    );
                        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE p
                FROM plantio p
                INNER JOIN horta h ON h.Id = p.HortaId
                INNER JOIN especie e ON e.Id = p.EspecieId
                WHERE h.Nome = 'Horta Comunitaria Central'
                  AND h.Local = 'Rua das Flores, 120 - Centro'
                  AND e.Nome IN ('Alface', 'Tomate', 'Cenoura', 'Cebolinha');
            ");
        }
    }
}