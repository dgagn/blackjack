namespace V2;

public enum Decision
{
  Hit,
  Hold
}

[Serializable]
public record ModelDecision
{
  public ulong WinCount { get; set; }
  public ulong LostCount { get; set; }
  public ulong TieCount { get; set; }
  public ulong GameCount => WinCount + LostCount + TieCount;
  public float Points => MathF.Ceiling((WinCount / GameCount - LostCount / GameCount - TieCount / GameCount) * 100);
}
