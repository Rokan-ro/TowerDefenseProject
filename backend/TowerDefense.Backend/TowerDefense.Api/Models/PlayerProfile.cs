namespace TowerDefense.Api.Models;

public class PlayerProfile
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public long Coins { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;

    public ICollection<PlayerTowerUpgrade> TowerUpgrades { get; set; }
        = new List<PlayerTowerUpgrade>();

    public ICollection<MatchHistory> MatchHistories { get; set; }
        = new List<MatchHistory>();
}