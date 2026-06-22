namespace TowerDefense.Api.Models;

public class TowerConfiguration
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal BaseDamage { get; set; }

    public decimal BaseRange { get; set; }

    public decimal BaseAttackInterval { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PlayerTowerUpgrade> PlayerUpgrades { get; set; }
        = new List<PlayerTowerUpgrade>();
}