namespace Blackjack;

public class Deck
{
  public readonly Stack<Card> Hand = new();

  public Deck() {
  }

  private Deck(Stack<Card> hand) {
    Hand = hand;
  }

  public static Deck Generate() {
    Stack<Card> cards = new();

    var ranks = new[] {
      "A",
      "2",
      "3",
      "4",
      "5",
      "6",
      "7",
      "8",
      "9",
      "10",
      "J",
      "Q",
      "K"
    };

    var suits = new[] {
      "hearts",
      "spades",
      "clubs",
      "diamonds"
    };

    foreach (var rank in ranks)
    foreach (var _ in suits)
      cards.Push(new Card(rank));

    return new Deck(cards);
  }

  public int HandValue() {
    var values = new Dictionary<string, int> {
      { "A", 1 },
      { "2", 2 },
      { "3", 3 },
      { "4", 4 },
      { "5", 5 },
      { "6", 6 },
      { "7", 7 },
      { "8", 8 },
      { "9", 9 },
      { "10", 10 },
      { "J", 10 },
      { "Q", 10 },
      { "K", 10 }
    };

    var value = Hand.Sum(card => values[card.Rank]);

    foreach (var _ in Hand.Where(card => card.Rank == "A" && value < 12))
      value += 10;

    return value;
  }
}