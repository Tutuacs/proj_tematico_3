using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedSegundaHortaMembros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Completa o quadro da Horta Comunitaria Central (admin1 + user1 + user2)
                INSERT INTO membro (HortaId, PerfilId, Role)
                SELECT h.Id, p.Id, 1
                FROM horta h
                INNER JOIN profile p ON p.Email = 'user1@gmail.com'
                WHERE h.Nome = 'Horta Comunitaria Central'
                  AND h.Local = 'Rua das Flores, 120 - Centro'
                  AND NOT EXISTS (
                      SELECT 1 FROM membro m WHERE m.HortaId = h.Id AND m.PerfilId = p.Id
                  );

                INSERT INTO membro (HortaId, PerfilId, Role)
                SELECT h.Id, p.Id, 1
                FROM horta h
                INNER JOIN profile p ON p.Email = 'user2@gmail.com'
                WHERE h.Nome = 'Horta Comunitaria Central'
                  AND h.Local = 'Rua das Flores, 120 - Centro'
                  AND NOT EXISTS (
                      SELECT 1 FROM membro m WHERE m.HortaId = h.Id AND m.PerfilId = p.Id
                  );

                -- Nova horta: Horta da Escola Municipal (admin2 + user2)
                INSERT INTO horta (Nome, Descricao, Local, Largura, Profundidade, Status, CreatedAt)
                SELECT 'Horta da Escola Municipal', 'Horta pedagogica mantida pelos alunos e equipe da escola.', 'Av. Brasil, 540 - Bairro Jardim', 8.0, 4.0, 0, CURRENT_DATE()
                WHERE NOT EXISTS (
                    SELECT 1 FROM horta WHERE Nome = 'Horta da Escola Municipal' AND Local = 'Av. Brasil, 540 - Bairro Jardim'
                );

                INSERT INTO membro (HortaId, PerfilId, Role)
                SELECT h.Id, p.Id, 0
                FROM horta h
                INNER JOIN profile p ON p.Email = 'admin2@gmail.com'
                WHERE h.Nome = 'Horta da Escola Municipal'
                  AND h.Local = 'Av. Brasil, 540 - Bairro Jardim'
                  AND NOT EXISTS (
                      SELECT 1 FROM membro m WHERE m.HortaId = h.Id AND m.PerfilId = p.Id
                  );

                INSERT INTO membro (HortaId, PerfilId, Role)
                SELECT h.Id, p.Id, 1
                FROM horta h
                INNER JOIN profile p ON p.Email = 'user2@gmail.com'
                WHERE h.Nome = 'Horta da Escola Municipal'
                  AND h.Local = 'Av. Brasil, 540 - Bairro Jardim'
                  AND NOT EXISTS (
                      SELECT 1 FROM membro m WHERE m.HortaId = h.Id AND m.PerfilId = p.Id
                  );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE m FROM membro m
                INNER JOIN horta h ON h.Id = m.HortaId
                INNER JOIN profile p ON p.Id = m.PerfilId
                WHERE h.Nome = 'Horta da Escola Municipal'
                  AND h.Local = 'Av. Brasil, 540 - Bairro Jardim';

                DELETE FROM horta
                WHERE Nome = 'Horta da Escola Municipal' AND Local = 'Av. Brasil, 540 - Bairro Jardim';

                DELETE m FROM membro m
                INNER JOIN horta h ON h.Id = m.HortaId
                INNER JOIN profile p ON p.Id = m.PerfilId
                WHERE h.Nome = 'Horta Comunitaria Central'
                  AND h.Local = 'Rua das Flores, 120 - Centro'
                  AND p.Email IN ('user1@gmail.com', 'user2@gmail.com');
            ");
        }
    }
}