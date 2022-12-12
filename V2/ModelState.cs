namespace V2;

[Serializable]
public record ModelState(int PlayerScore, int DealerScore, int DeckCount)
{
  public int PlayerScore { get; private set; } = PlayerScore;
  public int DealerScore { get; private set; } = DealerScore;
  public int DeckCount { get; private set; } = DeckCount;
}
