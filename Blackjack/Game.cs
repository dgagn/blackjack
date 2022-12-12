namespace Blackjack;

public class Game
{
  public readonly Guid Uuid = Guid.NewGuid();
  public readonly Deck Dealer = new();
  public Deck Deck = Deck.Generate();
  public readonly Deck Player = new();
  public int Cash = 1000;
  public GameState State = GameState.Idle;
  private int _bet = 10;

  public void Deal(int betDeal) {
    _bet = betDeal;
    if (State is GameState.Won or GameState.Lost or GameState.Tie)
      State = GameState.Idle;

    if (State != GameState.Idle)
      throw new InvalidOperationException("Cannot deal");

    if (_bet is < 0 or > 50)
      throw new InvalidOperationException(
        $"Cannot bet over the limit of 50 or under 0 given {_bet}");

    if (Cash < _bet)
      throw new InvalidOperationException(
        $"Cannot bet more than the available cash {Cash}");

    if (Deck.Hand.Count < 18) {
      Deck = Deck.Generate();
    }
    
    Dealer.Hand.Push(Deck.Hand.Pop());
    Player.Hand.Push(Deck.Hand.Pop());
    Player.Hand.Push(Deck.Hand.Pop());

    State = GameState.InGame;
    Cash -= _bet;

    var playerValue = Player.HandValue();
    var dealerValue = Dealer.HandValue();

    if (playerValue != 21 || dealerValue >= 10) return;

    State = GameState.Won;
    Cash += 3 * _bet;
  }

  public void Hit() {
    if (State != GameState.InGame)
      throw new InvalidOperationException(
        "Cannot hit when no game is started.");

    Player.Hand.Push(Deck.Hand.Pop());
    var playerValue = Player.HandValue();
    if (playerValue > 21) State = GameState.Lost;
    else {
      Hold();
    }
  }

  public void Hold() {
    if (State != GameState.InGame)
      throw new InvalidOperationException(
        "Cannot hit when no game is started.");

    var playerValue = Player.HandValue();
    var dealerValue = Dealer.HandValue();

    while (dealerValue < 17 && dealerValue <= playerValue) {
      Dealer.Hand.Push(Deck.Hand.Pop());
      dealerValue = Dealer.HandValue();
    }

    if (playerValue == dealerValue) {
      State = GameState.Tie;
      Cash += _bet;
    }
    else if (playerValue == 21) {
      State = GameState.Won;
      Cash = 2 * _bet;
    }
    else if (dealerValue > 21) {
      State = GameState.Won;
      Cash = 2 * _bet;
    }
    else if (playerValue > dealerValue) {
      State = GameState.Won;
      Cash = 2 * _bet;
    }
    else {
      State = GameState.Lost;
    }
  }

  public override string ToString() {
    return $"Game\nCash: {Cash}\nPlayer: {Player.HandValue()}\nDealer: {Dealer.HandValue()}\nState: {State}";
  }
}