using System.Data;
using Blackjack;

namespace V1;

public class V1
{
  public readonly Model?[,] Models;
  private const int MinGame = 100_000;

  public V1(Model?[,] models) {
    Models = models;
  }
  
  public Decision MakeChoice(ModelState state) {
    var playerValue = state.PlayerScore;
    var dealerValue = state.DealerScore;
    
    switch (playerValue) {
      case < 11:
        return Decision.Hit;
      case 21:
        return Decision.Hold;
    }

    var model = Models[playerValue, dealerValue];
    if (model == null) {
      Models[playerValue, dealerValue] = new Model {
        PlayerScore = playerValue,
        DealerScore = dealerValue,
      };
      return Decision.Hold;
    }

    if (model.Hold.GameCount < MinGame) {
      return Decision.Hold;
    }

    if (model.Hit.GameCount < MinGame) {
      return Decision.Hit;
    }

    return model.Hit.Points > model.Hold.Points ? Decision.Hit : Decision.Hold;
  }

  public void ProcessDecision(ModelState ms, Decision decision, GameState state) {
    var model = Models[ms.PlayerScore, ms.DealerScore];
    if (model == null) {
      return;
    }
    
    switch (state) {
      case GameState.Won when decision == Decision.Hit:
        model.Hit.WinCount++;
        break;
      case GameState.Won when decision == Decision.Hold:
        model.Hold.WinCount++;
        break;
      case GameState.Tie when decision == Decision.Hit:
        model.Hit.TieCount++;
        break;
      case GameState.Tie when decision == Decision.Hold:
        model.Hold.TieCount++;
        break;
      case GameState.Lost when decision == Decision.Hit:
        model.Hit.LostCount++;
        break;
      case GameState.Lost when decision == Decision.Hold:
        model.Hold.LostCount++;
        break;
      case GameState.Idle:
      case GameState.InGame:
      default:
        break;
    }
  }
}