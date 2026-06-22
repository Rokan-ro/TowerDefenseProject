namespace TowerDefense.Api.Models;

public class PlayerTowerUpgrade
{
    public long Id { get; set; }

    public long PlayerProfileId { get; set; }

    public long TowerConfigurationId { get; set; }

    public int UpgradeLevel { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public PlayerProfile PlayerProfile { get; set; } = null!;

    public TowerConfiguration TowerConfiguration { get; set; } = null!;
}