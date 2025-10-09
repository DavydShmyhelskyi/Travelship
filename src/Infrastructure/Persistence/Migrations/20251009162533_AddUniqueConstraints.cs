using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_users_nick_name",
                table: "users",
                column: "nick_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_roles_title",
                table: "roles",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_countries_title",
                table: "countries",
                column: "title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_nick_name",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_roles_title",
                table: "roles");

            migrationBuilder.DropIndex(
                name: "ix_countries_title",
                table: "countries");
        }
    }
}
