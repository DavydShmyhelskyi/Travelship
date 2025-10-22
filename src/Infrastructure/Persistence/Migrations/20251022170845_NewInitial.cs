using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "places",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false),
                    latitude = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false),
                    longitude = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_places", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false),
                    country_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cities", x => x.id);
                    table.ForeignKey(
                        name: "fk_cities_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "place_photos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    photo = table.Column<byte[]>(type: "bytea", nullable: false),
                    description = table.Column<string>(type: "varchar(255)", nullable: false),
                    is_shown = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    place_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_place_photos", x => x.id);
                    table.ForeignKey(
                        name: "fk_place_photos_places_place_id",
                        column: x => x.place_id,
                        principalTable: "places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nick_name = table.Column<string>(type: "varchar(100)", nullable: false),
                    avatar = table.Column<byte[]>(type: "bytea", nullable: true),
                    email = table.Column<string>(type: "varchar(255)", nullable: false),
                    password_hash = table.Column<string>(type: "varchar(255)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    city_id = table.Column<Guid>(type: "uuid", nullable: true),
                    city_id1 = table.Column<Guid>(type: "uuid", nullable: true),
                    role_id1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_users_cities_city_id1",
                        column: x => x.city_id1,
                        principalTable: "cities",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_users_roles_role_id1",
                        column: x => x.role_id1,
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "feedbacks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment = table.Column<string>(type: "varchar(500)", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    place_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feedbacks", x => x.id);
                    table.ForeignKey(
                        name: "fk_feedbacks_places_place_id",
                        column: x => x.place_id,
                        principalTable: "places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_feedbacks_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "followers",
                columns: table => new
                {
                    follower_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    followed_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_followers", x => new { x.follower_user_id, x.followed_user_id });
                    table.ForeignKey(
                        name: "fk_followers_users_followed_user_id",
                        column: x => x.followed_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_followers_users_follower_user_id",
                        column: x => x.follower_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "travels",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "varchar(1000)", nullable: false),
                    image = table.Column<byte[]>(type: "bytea", nullable: true),
                    is_done = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_travels", x => x.id);
                    table.ForeignKey(
                        name: "fk_travels_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "travel_places",
                columns: table => new
                {
                    travel_id = table.Column<Guid>(type: "uuid", nullable: false),
                    place_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_travel_places", x => new { x.travel_id, x.place_id });
                    table.ForeignKey(
                        name: "fk_travel_places_places_place_id",
                        column: x => x.place_id,
                        principalTable: "places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_travel_places_travels_travel_id",
                        column: x => x.travel_id,
                        principalTable: "travels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_travels",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    travel_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id1 = table.Column<Guid>(type: "uuid", nullable: true),
                    travel_id1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_travels", x => new { x.user_id, x.travel_id });
                    table.ForeignKey(
                        name: "fk_user_travels_travels_travel_id",
                        column: x => x.travel_id,
                        principalTable: "travels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_travels_travels_travel_id1",
                        column: x => x.travel_id1,
                        principalTable: "travels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_travels_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_travels_users_user_id1",
                        column: x => x.user_id1,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_cities_country_id",
                table: "cities",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_countries_title",
                table: "countries",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_feedbacks_place_id",
                table: "feedbacks",
                column: "place_id");

            migrationBuilder.CreateIndex(
                name: "ix_feedbacks_user_id",
                table: "feedbacks",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_followers_followed_user_id",
                table: "followers",
                column: "followed_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_place_photos_place_id",
                table: "place_photos",
                column: "place_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_title",
                table: "roles",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_travel_places_place_id",
                table: "travel_places",
                column: "place_id");

            migrationBuilder.CreateIndex(
                name: "ix_travels_user_id",
                table: "travels",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_travels_travel_id",
                table: "user_travels",
                column: "travel_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_travels_travel_id1",
                table: "user_travels",
                column: "travel_id1");

            migrationBuilder.CreateIndex(
                name: "ix_user_travels_user_id",
                table: "user_travels",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "ix_users_city_id",
                table: "users",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_city_id1",
                table: "users",
                column: "city_id1");

            migrationBuilder.CreateIndex(
                name: "ix_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_role_id1",
                table: "users",
                column: "role_id1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feedbacks");

            migrationBuilder.DropTable(
                name: "followers");

            migrationBuilder.DropTable(
                name: "place_photos");

            migrationBuilder.DropTable(
                name: "travel_places");

            migrationBuilder.DropTable(
                name: "user_travels");

            migrationBuilder.DropTable(
                name: "places");

            migrationBuilder.DropTable(
                name: "travels");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}
