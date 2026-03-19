using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedProfileMembroHortaTeste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET @seed_profile_id = 'seed-profile-teste';

                INSERT INTO profile (Id, Name, Email, Password, CreatedAt)
                SELECT @seed_profile_id, 'teste', 'teste@teste.com', '$100000$ABEiM0RVZneImaq7zN3u/w==$qmWhoT4Hk5a0iT2r7XbufmJ7qVynlgHh3K8gM51eAmM=', CURRENT_DATE()
                WHERE NOT EXISTS (
                    SELECT 1 FROM profile WHERE Email = 'teste@teste.com'
                );

                INSERT INTO horta (Nome, Descricao, Local, Largura, Profundidade, Status, CreatedAt)
                SELECT 'Horta Teste', 'Horta de teste', 'Local de teste', 10.0, 5.0, 0, CURRENT_DATE()
                WHERE NOT EXISTS (
                    SELECT 1 FROM horta WHERE Nome = 'Horta Teste' AND Local = 'Local de teste'
                );

                INSERT INTO membro (HortaId, PerfilId, Role)
                SELECT h.Id, p.Id, 1
                FROM horta h
                INNER JOIN profile p ON p.Email = 'teste@teste.com'
                WHERE h.Nome = 'Horta Teste'
                  AND h.Local = 'Local de teste'
                  AND NOT EXISTS (
                      SELECT 1 FROM membro m
                      WHERE m.HortaId = h.Id AND m.PerfilId = p.Id
                  );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM membro
                WHERE PerfilId IN (
                    SELECT Id FROM profile WHERE Email = 'teste@teste.com'
                );

                DELETE FROM horta
                WHERE Nome = 'Horta Teste' AND Local = 'Local de teste';

                DELETE FROM profile
                WHERE Email = 'teste@teste.com';
            ");
        }
    }
}
