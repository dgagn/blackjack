using Blackjack;

namespace V2;

[Serializable]
public record Model
{
  public int PlayerScore { get; set; }
  public int DealerScore { get; set; }
  
  public int DeckScore { get; set; }

  public ModelDecision Hit { get; init; } = new();
  public ModelDecision Hold { get; init; } = new();

  public override string ToString() {
    return $"{PlayerScore},{DealerScore},{DeckScore},{Hit.WinCount},{Hit.TieCount},{Hit.LostCount},{Hold.WinCount},{Hold.TieCount},{Hold.LostCount}";
  }
}
