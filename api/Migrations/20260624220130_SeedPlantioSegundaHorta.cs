using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedPlantioSegundaHorta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO plantio (EspecieId, HortaId, DataPlantio, Quantidade, Status, CreatedAt)
                SELECT e.Id, h.Id, DATE_SUB(CURRENT_DATE(), INTERVAL 5 DAY), 15, 0, CURRENT_DATE()
                FROM especie e
                INNER JOIN horta h ON h.Nome = 'Horta da Escola Municipal' AND h.Local = 'Av. Brasil, 540 - Bairro Jardim'
                WHERE e.Nome = 'Tomate'
                  AND NOT EXISTS (
                      SELECT 1 FROM plantio p WHERE p.EspecieId = e.Id AND p.HortaId = h.Id
                  );

                INSERT INTO plantio (EspecieId, HortaId, DataPlantio, Quantidade, Status, CreatedAt)
                SELECT e.Id, h.Id, DATE_SUB(CURRENT_DATE(), INTERVAL 20 DAY), 25, 0, CURRENT_DATE()
                FROM especie e
                INNER JOIN horta h ON h.Nome = 'Horta da Escola Municipal' AND h.Local = 'Av. Brasil, 540 - Bairro Jardim'
                WHERE e.Nome = 'Cenoura'
                  AND NOT EXISTS (
                      SELECT 1 FROM plantio p WHERE p.EspecieId = e.Id AND p.HortaId = h.Id
                  );

                INSERT INTO plantio (EspecieId, HortaId, DataPlantio, Quantidade, Status, CreatedAt)
                SELECT e.Id, h.Id, CURRENT_DATE(), 10, 0, CURRENT_DATE()
                FROM especie e
                INNER JOIN horta h ON h.Nome = 'Horta da Escola Municipal' AND h.Local = 'Av. Brasil, 540 - Bairro Jardim'
                WHERE e.Nome = 'Cebolinha'
                  AND NOT EXISTS (
                      SELECT 1 FROM plantio p WHERE p.EspecieId = e.Id AND p.HortaId = h.Id
                  );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE p FROM plantio p
                INNER JOIN horta h ON h.Id = p.HortaId
                INNER JOIN especie e ON e.Id = p.EspecieId
                WHERE h.Nome = 'Horta da Escola Municipal'
                  AND h.Local = 'Av. Brasil, 540 - Bairro Jardim'
                  AND e.Nome IN ('Tomate', 'Cenoura', 'Cebolinha');
            ");
        }
    }
}