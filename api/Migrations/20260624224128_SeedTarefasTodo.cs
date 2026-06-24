using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedTarefasTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Horta Comunitaria Central: regar o plantio de alface (aberta, sem responsavel ainda)
                INSERT INTO todo (Tipo, Descricao, DataLimite, PlantioId, HortaId, MembroId, CompletedAt, Status, CreatedAt)
                SELECT 0, 'Regar o canteiro de alface pela manha', CURRENT_DATE(), p.Id, h.Id, NULL, NULL, 0, CURRENT_DATE()
                FROM horta h
                INNER JOIN plantio p ON p.HortaId = h.Id
                INNER JOIN especie e ON e.Id = p.EspecieId AND e.Nome = 'Alface'
                WHERE h.Nome = 'Horta Comunitaria Central' AND h.Local = 'Rua das Flores, 120 - Centro'
                  AND NOT EXISTS (
                      SELECT 1 FROM todo t WHERE t.PlantioId = p.Id AND t.Tipo = 0 AND t.Status = 0
                  );

                -- Horta Comunitaria Central: inspecionar tomates, em andamento, com responsavel (user1)
                INSERT INTO todo (Tipo, Descricao, DataLimite, PlantioId, HortaId, MembroId, CompletedAt, Status, CreatedAt)
                SELECT 5, 'Verificar sinais de pragas nos pes de tomate', DATE_ADD(CURRENT_DATE(), INTERVAL 2 DAY), p.Id, h.Id, m.Id, NULL, 1, CURRENT_DATE()
                FROM horta h
                INNER JOIN plantio p ON p.HortaId = h.Id
                INNER JOIN especie e ON e.Id = p.EspecieId AND e.Nome = 'Tomate'
                INNER JOIN membro m ON m.HortaId = h.Id
                INNER JOIN profile pr ON pr.Id = m.PerfilId AND pr.Email = 'user1@gmail.com'
                WHERE h.Nome = 'Horta Comunitaria Central' AND h.Local = 'Rua das Flores, 120 - Centro'
                  AND NOT EXISTS (
                      SELECT 1 FROM todo t WHERE t.PlantioId = p.Id AND t.Tipo = 5
                  );

                -- Horta Comunitaria Central: colher cenoura, concluida (com responsavel user2 e data de conclusao)
                INSERT INTO todo (Tipo, Descricao, DataLimite, PlantioId, HortaId, MembroId, CompletedAt, Status, CreatedAt)
                SELECT 2, 'Colher cenouras que ja atingiram o ponto ideal', DATE_SUB(CURRENT_DATE(), INTERVAL 1 DAY), p.Id, h.Id, m.Id, CURRENT_DATE(), 2, DATE_SUB(CURRENT_DATE(), INTERVAL 5 DAY)
                FROM horta h
                INNER JOIN plantio p ON p.HortaId = h.Id
                INNER JOIN especie e ON e.Id = p.EspecieId AND e.Nome = 'Cenoura'
                INNER JOIN membro m ON m.HortaId = h.Id
                INNER JOIN profile pr ON pr.Id = m.PerfilId AND pr.Email = 'user2@gmail.com'
                WHERE h.Nome = 'Horta Comunitaria Central' AND h.Local = 'Rua das Flores, 120 - Centro'
                  AND NOT EXISTS (
                      SELECT 1 FROM todo t WHERE t.PlantioId = p.Id AND t.Tipo = 2
                  );

                -- Horta Comunitaria Central: tarefa geral (sem plantio), limpar canteiros, aberta
                INSERT INTO todo (Tipo, Descricao, DataLimite, PlantioId, HortaId, MembroId, CompletedAt, Status, CreatedAt)
                SELECT 3, 'Remover ervas daninhas dos canteiros vazios', DATE_ADD(CURRENT_DATE(), INTERVAL 4 DAY), NULL, h.Id, NULL, NULL, 0, CURRENT_DATE()
                FROM horta h
                WHERE h.Nome = 'Horta Comunitaria Central' AND h.Local = 'Rua das Flores, 120 - Centro'
                  AND NOT EXISTS (
                      SELECT 1 FROM todo t WHERE t.HortaId = h.Id AND t.PlantioId IS NULL AND t.Tipo = 3
                  );

                -- Horta da Escola Municipal: adubar tomate, aberta, com responsavel admin2
                INSERT INTO todo (Tipo, Descricao, DataLimite, PlantioId, HortaId, MembroId, CompletedAt, Status, CreatedAt)
                SELECT 4, 'Aplicar adubo organico nos pes de tomate', DATE_ADD(CURRENT_DATE(), INTERVAL 3 DAY), p.Id, h.Id, m.Id, NULL, 0, CURRENT_DATE()
                FROM horta h
                INNER JOIN plantio p ON p.HortaId = h.Id
                INNER JOIN especie e ON e.Id = p.EspecieId AND e.Nome = 'Tomate'
                INNER JOIN membro m ON m.HortaId = h.Id
                INNER JOIN profile pr ON pr.Id = m.PerfilId AND pr.Email = 'admin2@gmail.com'
                WHERE h.Nome = 'Horta da Escola Municipal' AND h.Local = 'Av. Brasil, 540 - Bairro Jardim'
                  AND NOT EXISTS (
                      SELECT 1 FROM todo t WHERE t.PlantioId = p.Id AND t.Tipo = 4
                  );

                -- Horta da Escola Municipal: poda da cebolinha, cancelada (com responsavel user2)
                INSERT INTO todo (Tipo, Descricao, DataLimite, PlantioId, HortaId, MembroId, CompletedAt, Status, CreatedAt)
                SELECT 6, 'Podar excesso de folhas da cebolinha', DATE_SUB(CURRENT_DATE(), INTERVAL 2 DAY), p.Id, h.Id, m.Id, NULL, 3, DATE_SUB(CURRENT_DATE(), INTERVAL 6 DAY)
                FROM horta h
                INNER JOIN plantio p ON p.HortaId = h.Id
                INNER JOIN especie e ON e.Id = p.EspecieId AND e.Nome = 'Cebolinha'
                INNER JOIN membro m ON m.HortaId = h.Id
                INNER JOIN profile pr ON pr.Id = m.PerfilId AND pr.Email = 'user2@gmail.com'
                WHERE h.Nome = 'Horta da Escola Municipal' AND h.Local = 'Av. Brasil, 540 - Bairro Jardim'
                  AND NOT EXISTS (
                      SELECT 1 FROM todo t WHERE t.PlantioId = p.Id AND t.Tipo = 6
                  );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE t FROM todo t
                INNER JOIN horta h ON h.Id = t.HortaId
                WHERE (h.Nome = 'Horta Comunitaria Central' AND h.Local = 'Rua das Flores, 120 - Centro')
                   OR (h.Nome = 'Horta da Escola Municipal' AND h.Local = 'Av. Brasil, 540 - Bairro Jardim');
            ");
        }
    }
}