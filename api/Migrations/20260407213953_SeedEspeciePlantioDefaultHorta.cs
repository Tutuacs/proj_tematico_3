using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedEspeciePlantioDefaultHorta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO especie (Nome, ImageLink, DiasParaRegar, DiasParaColher, MesesPlantio, MesesColheita, Descricao)
                SELECT 'Alface', 'https://static.vecteezy.com/system/resources/thumbnails/046/438/320/small/lettuce-transparent-background-png.png', 2, 45, '3,4,5,6', '5,6,7,8', 'Folhosa de crescimento rápido.'
                WHERE NOT EXISTS (
                    SELECT 1 FROM especie WHERE Nome = 'Alface'
                );

                INSERT INTO especie (Nome, ImageLink, DiasParaRegar, DiasParaColher, MesesPlantio, MesesColheita, Descricao)
                SELECT 'Tomate', 'https://static.vecteezy.com/system/resources/thumbnails/024/731/923/small/red-tomatoes-isolated-on-transparent-background-created-with-generative-ai-png.png', 2, 90, '8,9,10,11', '12,1,2,3', 'Frutífera bastante comum em hortas comunitárias.'
                WHERE NOT EXISTS (
                    SELECT 1 FROM especie WHERE Nome = 'Tomate'
                );

                INSERT INTO especie (Nome, ImageLink, DiasParaRegar, DiasParaColher, MesesPlantio, MesesColheita, Descricao)
                SELECT 'Cenoura', 'https://static.vecteezy.com/system/resources/thumbnails/058/218/265/small/a-single-vibrant-orange-carrot-with-its-green-leafy-top-png.png', 3, 80, '3,4,5,6,7', '6,7,8,9,10', 'Raiz de ciclo médio e boa adaptação.'
                WHERE NOT EXISTS (
                    SELECT 1 FROM especie WHERE Nome = 'Cenoura'
                );

                INSERT INTO especie (Nome, ImageLink, DiasParaRegar, DiasParaColher, MesesPlantio, MesesColheita, Descricao)
                SELECT 'Cebolinha', 'https://static.vecteezy.com/system/resources/thumbnails/046/438/236/small/green-onion-transparent-background-png.png', 2, 60, '1,2,3,4,5,6,7,8,9,10,11,12', '1,2,3,4,5,6,7,8,9,10,11,12', 'Erva aromática de manejo simples.'
                WHERE NOT EXISTS (
                    SELECT 1 FROM especie WHERE Nome = 'Cebolinha'
                );

                INSERT INTO plantio (EspecieId, HortaId, DataPlantio, Quantidade, Status, CreatedAt)
                SELECT e.Id, h.Id, CURRENT_DATE(), 20, 0, CURRENT_DATE()
                FROM especie e
                INNER JOIN horta h ON h.Nome = 'Horta Teste' AND h.Local = 'Local de teste'
                WHERE e.Nome = 'Alface'
                  AND NOT EXISTS (
                      SELECT 1 FROM plantio p
                      WHERE p.EspecieId = e.Id AND p.HortaId = h.Id
                  );

                INSERT INTO plantio (EspecieId, HortaId, DataPlantio, Quantidade, Status, CreatedAt)
                SELECT e.Id, h.Id, DATE_SUB(CURRENT_DATE(), INTERVAL 7 DAY), 12, 0, CURRENT_DATE()
                FROM especie e
                INNER JOIN horta h ON h.Nome = 'Horta Teste' AND h.Local = 'Local de teste'
                WHERE e.Nome = 'Tomate'
                  AND NOT EXISTS (
                      SELECT 1 FROM plantio p
                      WHERE p.EspecieId = e.Id AND p.HortaId = h.Id
                  );

                INSERT INTO plantio (EspecieId, HortaId, DataPlantio, Quantidade, Status, CreatedAt)
                SELECT e.Id, h.Id, DATE_SUB(CURRENT_DATE(), INTERVAL 15 DAY), 30, 0, CURRENT_DATE()
                FROM especie e
                INNER JOIN horta h ON h.Nome = 'Horta Teste' AND h.Local = 'Local de teste'
                WHERE e.Nome = 'Cenoura'
                  AND NOT EXISTS (
                      SELECT 1 FROM plantio p
                      WHERE p.EspecieId = e.Id AND p.HortaId = h.Id
                  );

                INSERT INTO plantio (EspecieId, HortaId, DataPlantio, Quantidade, Status, CreatedAt)
                SELECT e.Id, h.Id, DATE_SUB(CURRENT_DATE(), INTERVAL 3 DAY), 18, 0, CURRENT_DATE()
                FROM especie e
                INNER JOIN horta h ON h.Nome = 'Horta Teste' AND h.Local = 'Local de teste'
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
                WHERE h.Nome = 'Horta Teste'
                  AND h.Local = 'Local de teste'
                  AND e.Nome IN ('Alface', 'Tomate', 'Cenoura', 'Cebolinha')
                  AND e.ImageLink IN (
                      'https://static.vecteezy.com/system/resources/thumbnails/046/438/320/small/lettuce-transparent-background-png.png',
                      'https://static.vecteezy.com/system/resources/thumbnails/024/731/923/small/red-tomatoes-isolated-on-transparent-background-created-with-generative-ai-png.png',
                      'https://static.vecteezy.com/system/resources/thumbnails/058/218/265/small/a-single-vibrant-orange-carrot-with-its-green-leafy-top-png.png',
                      'https://static.vecteezy.com/system/resources/thumbnails/046/438/236/small/green-onion-transparent-background-png.png'
                  );

                DELETE FROM especie
                WHERE Nome IN ('Alface', 'Tomate', 'Cenoura', 'Cebolinha')
                  AND ImageLink IN (
                      'https://static.vecteezy.com/system/resources/thumbnails/046/438/320/small/lettuce-transparent-background-png.png',
                      'https://static.vecteezy.com/system/resources/thumbnails/024/731/923/small/red-tomatoes-isolated-on-transparent-background-created-with-generative-ai-png.png',
                      'https://static.vecteezy.com/system/resources/thumbnails/058/218/265/small/a-single-vibrant-orange-carrot-with-its-green-leafy-top-png.png',
                      'https://static.vecteezy.com/system/resources/thumbnails/046/438/236/small/green-onion-transparent-background-png.png'
                  );
            ");
        }
    }
}
