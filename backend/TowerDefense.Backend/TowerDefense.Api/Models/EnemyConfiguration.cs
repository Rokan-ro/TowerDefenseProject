namespace TowerDefense.Api.Models;

public class EnemyConfiguration
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal BaseHealth { get; set; }

    public decimal MoveSpeed { get; set; }

    public int DamageToPlayer { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}