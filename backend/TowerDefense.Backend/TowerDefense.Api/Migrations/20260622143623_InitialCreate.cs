using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace TowerDefense.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "enemy_configurations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    base_health = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    move_speed = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    damage_to_player = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enemy_configurations", x => x.id);
                    table.CheckConstraint("CK_enemy_base_health", "`base_health` > 0");
                    table.CheckConstraint("CK_enemy_damage_to_player", "`damage_to_player` > 0");
                    table.CheckConstraint("CK_enemy_move_speed", "`move_speed` > 0");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tower_configurations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    base_damage = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    base_range = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    base_attack_interval = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tower_configurations", x => x.id);
                    table.CheckConstraint("CK_tower_attack_interval", "`base_attack_interval` > 0");
                    table.CheckConstraint("CK_tower_base_damage", "`base_damage` > 0");
                    table.CheckConstraint("CK_tower_base_range", "`base_range` > 0");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.CheckConstraint("CK_users_role", "`role` IN ('Player', 'Administrator')");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_profiles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    display_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    coins = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_profiles", x => x.id);
                    table.CheckConstraint("CK_player_profiles_coins", "`coins` >= 0");
                    table.ForeignKey(
                        name: "FK_player_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "match_histories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    player_profile_id = table.Column<long>(type: "bigint", nullable: false),
                    result = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    coins_reward = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    started_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ended_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_match_histories", x => x.id);
                    table.CheckConstraint("CK_match_coins_reward", "`coins_reward` >= 0");
                    table.CheckConstraint("CK_match_result", "`result` IN ('Win', 'Lose')");
                    table.CheckConstraint("CK_match_time", "`ended_at` >= `started_at`");
                    table.ForeignKey(
                        name: "FK_match_histories_player_profiles_player_profile_id",
                        column: x => x.player_profile_id,
                        principalTable: "player_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_tower_upgrades",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    player_profile_id = table.Column<long>(type: "bigint", nullable: false),
                    tower_configuration_id = table.Column<long>(type: "bigint", nullable: false),
                    upgrade_level = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_tower_upgrades", x => x.id);
                    table.CheckConstraint("CK_tower_upgrade_level", "`upgrade_level` >= 0");
                    table.ForeignKey(
                        name: "FK_player_tower_upgrades_player_profiles_player_profile_id",
                        column: x => x.player_profile_id,
                        principalTable: "player_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_player_tower_upgrades_tower_configurations_tower_configurati~",
                        column: x => x.tower_configuration_id,
                        principalTable: "tower_configurations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_enemy_configurations_name",
                table: "enemy_configurations",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_match_histories_ended_at",
                table: "match_histories",
                column: "ended_at");

            migrationBuilder.CreateIndex(
                name: "IX_match_histories_player_profile_id",
                table: "match_histories",
                column: "player_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_profiles_user_id",
                table: "player_profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_tower_upgrades_player_profile_id_tower_configuration_~",
                table: "player_tower_upgrades",
                columns: new[] { "player_profile_id", "tower_configuration_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_tower_upgrades_tower_configuration_id",
                table: "player_tower_upgrades",
                column: "tower_configuration_id");

            migrationBuilder.CreateIndex(
                name: "IX_tower_configurations_name",
                table: "tower_configurations",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_is_active",
                table: "users",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_users_role",
                table: "users",
                column: "role");

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enemy_configurations");

            migrationBuilder.DropTable(
                name: "match_histories");

            migrationBuilder.DropTable(
                name: "player_tower_upgrades");

            migrationBuilder.DropTable(
                name: "player_profiles");

            migrationBuilder.DropTable(
                name: "tower_configurations");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
