using Microsoft.EntityFrameworkCore;
using TowerDefense.Api.Models;

namespace TowerDefense.Api.Data;

public class TowerDefenseDbContext : DbContext
{
    public TowerDefenseDbContext(
        DbContextOptions<TowerDefenseDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<PlayerProfile> PlayerProfiles
        => Set<PlayerProfile>();

    public DbSet<TowerConfiguration> TowerConfigurations
        => Set<TowerConfiguration>();

    public DbSet<PlayerTowerUpgrade> PlayerTowerUpgrades
        => Set<PlayerTowerUpgrade>();

    public DbSet<EnemyConfiguration> EnemyConfigurations
        => Set<EnemyConfiguration>();

    public DbSet<MatchHistory> MatchHistories
        => Set<MatchHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUsers(modelBuilder);
        ConfigurePlayerProfiles(modelBuilder);
        ConfigureTowerConfigurations(modelBuilder);
        ConfigurePlayerTowerUpgrades(modelBuilder);
        ConfigureEnemyConfigurations(modelBuilder);
        ConfigureMatchHistories(modelBuilder);
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<User>();

        entity.ToTable(
            "users",
            table => table.HasCheckConstraint(
                "CK_users_role",
                "`role` IN ('Player', 'Administrator')"
            )
        );

        entity.HasKey(user => user.Id);

        entity.Property(user => user.Id)
            .HasColumnName("id");

        entity.Property(user => user.Username)
            .HasColumnName("username")
            .HasMaxLength(50)
            .IsRequired();

        entity.HasIndex(user => user.Username)
            .IsUnique();

        entity.Property(user => user.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255)
            .IsRequired();

        entity.Property(user => user.Role)
            .HasColumnName("role")
            .HasMaxLength(20)
            .IsRequired();

        entity.HasIndex(user => user.Role);

        entity.Property(user => user.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        entity.HasIndex(user => user.IsActive);

        entity.Property(user => user.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        entity.Property(user => user.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        entity.HasOne(user => user.PlayerProfile)
            .WithOne(profile => profile.User)
            .HasForeignKey<PlayerProfile>(
                profile => profile.UserId
            )
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigurePlayerProfiles(
        ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PlayerProfile>();

        entity.ToTable(
            "player_profiles",
            table => table.HasCheckConstraint(
                "CK_player_profiles_coins",
                "`coins` >= 0"
            )
        );

        entity.HasKey(profile => profile.Id);

        entity.Property(profile => profile.Id)
            .HasColumnName("id");

        entity.Property(profile => profile.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        entity.HasIndex(profile => profile.UserId)
            .IsUnique();

        entity.Property(profile => profile.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(profile => profile.Coins)
            .HasColumnName("coins")
            .HasDefaultValue(0L)
            .IsRequired();

        entity.Property(profile => profile.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        entity.Property(profile => profile.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }

    private static void ConfigureTowerConfigurations(
        ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<TowerConfiguration>();

        entity.ToTable(
            "tower_configurations",
            table =>
            {
                table.HasCheckConstraint(
                    "CK_tower_base_damage",
                    "`base_damage` > 0"
                );

                table.HasCheckConstraint(
                    "CK_tower_base_range",
                    "`base_range` > 0"
                );

                table.HasCheckConstraint(
                    "CK_tower_attack_interval",
                    "`base_attack_interval` > 0"
                );
            }
        );

        entity.HasKey(tower => tower.Id);

        entity.Property(tower => tower.Id)
            .HasColumnName("id");

        entity.Property(tower => tower.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        entity.HasIndex(tower => tower.Name)
            .IsUnique();

        entity.Property(tower => tower.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        entity.Property(tower => tower.BaseDamage)
            .HasColumnName("base_damage")
            .HasPrecision(10, 2)
            .IsRequired();

        entity.Property(tower => tower.BaseRange)
            .HasColumnName("base_range")
            .HasPrecision(10, 2)
            .IsRequired();

        entity.Property(tower => tower.BaseAttackInterval)
            .HasColumnName("base_attack_interval")
            .HasPrecision(10, 2)
            .IsRequired();

        entity.Property(tower => tower.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        entity.Property(tower => tower.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        entity.Property(tower => tower.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }

    private static void ConfigurePlayerTowerUpgrades(
        ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PlayerTowerUpgrade>();

        entity.ToTable(
            "player_tower_upgrades",
            table => table.HasCheckConstraint(
                "CK_tower_upgrade_level",
                "`upgrade_level` >= 0"
            )
        );

        entity.HasKey(upgrade => upgrade.Id);

        entity.Property(upgrade => upgrade.Id)
            .HasColumnName("id");

        entity.Property(upgrade => upgrade.PlayerProfileId)
            .HasColumnName("player_profile_id")
            .IsRequired();

        entity.Property(upgrade => upgrade.TowerConfigurationId)
            .HasColumnName("tower_configuration_id")
            .IsRequired();

        entity.Property(upgrade => upgrade.UpgradeLevel)
            .HasColumnName("upgrade_level")
            .HasDefaultValue(0)
            .IsRequired();

        entity.Property(upgrade => upgrade.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        entity.Property(upgrade => upgrade.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        entity.HasIndex(upgrade => new
        {
            upgrade.PlayerProfileId,
            upgrade.TowerConfigurationId
        })
        .IsUnique();

        entity.HasOne(upgrade => upgrade.PlayerProfile)
            .WithMany(profile => profile.TowerUpgrades)
            .HasForeignKey(upgrade => upgrade.PlayerProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(upgrade => upgrade.TowerConfiguration)
            .WithMany(tower => tower.PlayerUpgrades)
            .HasForeignKey(
                upgrade => upgrade.TowerConfigurationId
            )
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureEnemyConfigurations(
        ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<EnemyConfiguration>();

        entity.ToTable(
            "enemy_configurations",
            table =>
            {
                table.HasCheckConstraint(
                    "CK_enemy_base_health",
                    "`base_health` > 0"
                );

                table.HasCheckConstraint(
                    "CK_enemy_move_speed",
                    "`move_speed` > 0"
                );

                table.HasCheckConstraint(
                    "CK_enemy_damage_to_player",
                    "`damage_to_player` > 0"
                );
            }
        );

        entity.HasKey(enemy => enemy.Id);

        entity.Property(enemy => enemy.Id)
            .HasColumnName("id");

        entity.Property(enemy => enemy.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        entity.HasIndex(enemy => enemy.Name)
            .IsUnique();

        entity.Property(enemy => enemy.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        entity.Property(enemy => enemy.BaseHealth)
            .HasColumnName("base_health")
            .HasPrecision(10, 2)
            .IsRequired();

        entity.Property(enemy => enemy.MoveSpeed)
            .HasColumnName("move_speed")
            .HasPrecision(10, 2)
            .IsRequired();

        entity.Property(enemy => enemy.DamageToPlayer)
            .HasColumnName("damage_to_player")
            .IsRequired();

        entity.Property(enemy => enemy.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        entity.Property(enemy => enemy.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        entity.Property(enemy => enemy.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }

    private static void ConfigureMatchHistories(
        ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<MatchHistory>();

        entity.ToTable(
            "match_histories",
            table =>
            {
                table.HasCheckConstraint(
                    "CK_match_result",
                    "`result` IN ('Win', 'Lose')"
                );

                table.HasCheckConstraint(
                    "CK_match_coins_reward",
                    "`coins_reward` >= 0"
                );

                table.HasCheckConstraint(
                    "CK_match_time",
                    "`ended_at` >= `started_at`"
                );
            }
        );

        entity.HasKey(match => match.Id);

        entity.Property(match => match.Id)
            .HasColumnName("id");

        entity.Property(match => match.PlayerProfileId)
            .HasColumnName("player_profile_id")
            .IsRequired();

        entity.Property(match => match.Result)
            .HasColumnName("result")
            .HasMaxLength(10)
            .IsRequired();

        entity.Property(match => match.CoinsReward)
            .HasColumnName("coins_reward")
            .HasDefaultValue(0L)
            .IsRequired();

        entity.Property(match => match.StartedAt)
            .HasColumnName("started_at")
            .IsRequired();

        entity.Property(match => match.EndedAt)
            .HasColumnName("ended_at")
            .IsRequired();

        entity.HasIndex(match => match.PlayerProfileId);

        entity.HasIndex(match => match.EndedAt);

        entity.HasOne(match => match.PlayerProfile)
            .WithMany(profile => profile.MatchHistories)
            .HasForeignKey(match => match.PlayerProfileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}