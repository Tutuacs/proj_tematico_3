using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsuariosAdicionais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO profile (Id, Name, Email, Password, CreatedAt)
                SELECT 'admin2', 'Admin Regional', 'admin2@gmail.com', '$100000$ABEiM0RVZneImaq7zN3u/w==$qmWhoT4Hk5a0iT2r7XbufmJ7qVynlgHh3K8gM51eAmM=', CURRENT_DATE()
                WHERE NOT EXISTS (SELECT 1 FROM profile WHERE Email = 'admin2@gmail.com');

                INSERT INTO profile (Id, Name, Email, Password, CreatedAt)
                SELECT 'user1', 'Ana Souza', 'user1@gmail.com', '$100000$ABEiM0RVZneImaq7zN3u/w==$qmWhoT4Hk5a0iT2r7XbufmJ7qVynlgHh3K8gM51eAmM=', CURRENT_DATE()
                WHERE NOT EXISTS (SELECT 1 FROM profile WHERE Email = 'user1@gmail.com');

                INSERT INTO profile (Id, Name, Email, Password, CreatedAt)
                SELECT 'user2', 'Bruno Lima', 'user2@gmail.com', '$100000$ABEiM0RVZneImaq7zN3u/w==$qmWhoT4Hk5a0iT2r7XbufmJ7qVynlgHh3K8gM51eAmM=', CURRENT_DATE()
                WHERE NOT EXISTS (SELECT 1 FROM profile WHERE Email = 'user2@gmail.com');

                INSERT INTO profile (Id, Name, Email, Password, CreatedAt)
                SELECT 'user3', 'Carla Mendes', 'user3@gmail.com', '$100000$ABEiM0RVZneImaq7zN3u/w==$qmWhoT4Hk5a0iT2r7XbufmJ7qVynlgHh3K8gM51eAmM=', CURRENT_DATE()
                WHERE NOT EXISTS (SELECT 1 FROM profile WHERE Email = 'user3@gmail.com');

                INSERT INTO profile (Id, Name, Email, Password, CreatedAt)
                SELECT 'user4', 'Diego Pereira', 'user4@gmail.com', '$100000$ABEiM0RVZneImaq7zN3u/w==$qmWhoT4Hk5a0iT2r7XbufmJ7qVynlgHh3K8gM51eAmM=', CURRENT_DATE()
                WHERE NOT EXISTS (SELECT 1 FROM profile WHERE Email = 'user4@gmail.com');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM profile
                WHERE Email IN ('admin2@gmail.com', 'user1@gmail.com', 'user2@gmail.com', 'user3@gmail.com', 'user4@gmail.com');
            ");
        }
    }
}