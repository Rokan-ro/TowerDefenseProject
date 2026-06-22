namespace TowerDefense.Api.Models;

public class MatchHistory
{
    public long Id { get; set; }

    public long PlayerProfileId { get; set; }

    public string Result { get; set; } = string.Empty;

    public long CoinsReward { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime EndedAt { get; set; }

    public PlayerProfile PlayerProfile { get; set; } = null!;
}