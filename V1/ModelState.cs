namespace V1;

[Serializable]
public record ModelState(int PlayerScore, int DealerScore)
{
  public int PlayerScore { get; private set; } = PlayerScore;
  public int DealerScore { get; private set; } = DealerScore;
}
