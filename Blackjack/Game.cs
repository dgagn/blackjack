namespace Blackjack;

public class Game
{
  private readonly Guid _uuid = Guid.NewGuid();
  private readonly Deck _dealer = new();
  private Deck _deck = Deck.Generate();
  private readonly Deck _player = new();
  private int _cash = 1000;
  private GameState _state = GameState.Idle;
  private int _bet = 10;

  public void Deal(int betDeal) {
    _bet = betDeal;
    if (_state is GameState.Won or GameState.Lost or GameState.Tie)
      _state = GameState.Idle;

    if (_state != GameState.Idle)
      throw new InvalidOperationException("Cannot deal");

    if (_bet is < 0 or > 50)
      throw new InvalidOperationException(
        $"Cannot bet over the limit of 50 or under 0 given {_bet}");

    if (_cash < _bet)
      throw new InvalidOperationException(
        $"Cannot bet more than the available cash {_cash}");

    if (_deck.Hand.Count < 18) {
      _deck = Deck.Generate();
    }
    
    _dealer.Hand.Push(_deck.Hand.Pop());
    _player.Hand.Push(_deck.Hand.Pop());
    _player.Hand.Push(_deck.Hand.Pop());

    _state = GameState.InGame;
    _cash -= _bet;

    var playerValue = _player.HandValue();
    var dealerValue = _dealer.HandValue();

    if (playerValue != 21 || dealerValue >= 10) return;

    _state = GameState.Won;
    _cash += 3 * _bet;
  }

  public void Hit() {
    if (_state != GameState.InGame)
      throw new InvalidOperationException(
        "Cannot hit when no game is started.");

    _player.Hand.Push(_deck.Hand.Pop());
    var playerValue = _player.HandValue();
    if (playerValue > 21) _state = GameState.Lost;
  }

  public void Hold() {
    if (_state != GameState.InGame)
      throw new InvalidOperationException(
        "Cannot hit when no game is started.");

    var playerValue = _player.HandValue();
    var dealerValue = _dealer.HandValue();

    while (dealerValue < 17 && dealerValue <= playerValue) {
      _dealer.Hand.Push(_deck.Hand.Pop());
      dealerValue = _dealer.HandValue();
    }

    if (playerValue == dealerValue) {
      _state = GameState.Tie;
      _cash += _bet;
    }
    else if (playerValue == 21) {
      _state = GameState.Won;
      _cash = 2 * _bet;
    }
    else if (dealerValue > 21) {
      _state = GameState.Won;
      _cash = 2 * _bet;
    }
    else if (playerValue > dealerValue) {
      _state = GameState.Won;
      _cash = 2 * _bet;
    }
    else {
      _state = GameState.Lost;
    }
  }
}