using Blackjack;

namespace V1;

[Serializable]
public record Model
{
  public int PlayerScore { get; set; }
  public int DealerScore { get; set; }

  public ModelDecision Hit { get; init; } = new();
  public ModelDecision Hold { get; init; } = new();

  public override string ToString() {
    return $"{PlayerScore},{DealerScore},{Hit.WinCount},{Hit.TieCount},{Hit.LostCount},{Hold.WinCount},{Hold.TieCount},{Hold.LostCount}";
  }
}
