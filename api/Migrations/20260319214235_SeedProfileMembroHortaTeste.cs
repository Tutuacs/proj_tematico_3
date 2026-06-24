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
                SET @seed_profile_id = 'admin1';

                INSERT INTO profile (Id, Name, Email, Password, CreatedAt)
                SELECT @seed_profile_id, 'Admin Principal', 'admin1@gmail.com', '$100000$ABEiM0RVZneImaq7zN3u/w==$qmWhoT4Hk5a0iT2r7XbufmJ7qVynlgHh3K8gM51eAmM=', CURRENT_DATE()
                WHERE NOT EXISTS (
                    SELECT 1 FROM profile WHERE Email = 'admin1@gmail.com'
                );

                INSERT INTO horta (Nome, Descricao, Local, Largura, Profundidade, Status, CreatedAt)
                SELECT 'Horta Comunitaria Central', 'Horta comunitaria mantida pelo bairro, com canteiros coletivos.', 'Rua das Flores, 120 - Centro', 10.0, 5.0, 0, CURRENT_DATE()
                WHERE NOT EXISTS (
                    SELECT 1 FROM horta WHERE Nome = 'Horta Comunitaria Central' AND Local = 'Rua das Flores, 120 - Centro'
                );

                INSERT INTO membro (HortaId, PerfilId, Role)
                SELECT h.Id, p.Id, 0
                FROM horta h
                INNER JOIN profile p ON p.Email = 'admin1@gmail.com'
                WHERE h.Nome = 'Horta Comunitaria Central'
                  AND h.Local = 'Rua das Flores, 120 - Centro'
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
                    SELECT Id FROM profile WHERE Email = 'admin1@gmail.com'
                );

                DELETE FROM horta
                WHERE Nome = 'Horta Comunitaria Central' AND Local = 'Rua das Flores, 120 - Centro';

                DELETE FROM profile
                WHERE Email = 'admin1@gmail.com';
            ");
        }
    }
}