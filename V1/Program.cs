// See https://aka.ms/new-console-template for more information

using System.Text;
using Blackjack;
using V1;

const string filename = "v1.csv";

Model?[,] models = new Model[21,21];

var text = File.ReadAllText(filename, Encoding.UTF8)
  .Split("\n")
  .Skip(1)
  .Select(line => {
    var lines = line.Split(',');
    if (lines.Length != 8)
      throw new InvalidOperationException("Invalid CSV format");
    return new Model {
      PlayerScore = int.Parse(lines[0]),
      DealerScore = int.Parse(lines[1]),
      Hit = new ModelDecision {
        WinCount = ulong.Parse(lines[2]),
        TieCount = ulong.Parse(lines[3]),
        LostCount = ulong.Parse(lines[4]),
      },
      Hold = new ModelDecision {
        WinCount = ulong.Parse(lines[5]),
        TieCount = ulong.Parse(lines[6]),
        LostCount = ulong.Parse(lines[7])
      }
    };
  })
  .ToArray();

foreach (var line in text) {
  models[line.PlayerScore, line.DealerScore] = line;
}

var maker = new V1.V1(models);

var count = 0;

while (true) {
  var game = new Game();
  game.Deal(50);
  if (game.Player.HandValue() == 21) continue;
  var state = new ModelState(game.Player.HandValue(), game.Dealer.HandValue());
  var decision = maker.MakeChoice(
    state
  );
  if (decision == Decision.Hit)
    game.Hit();
  else
    game.Hold();
  
  maker.ProcessDecision(state, decision, game.State);

  count++;
  
  if (count % 500_000 == 0) {
    File.WriteAllText(filename, "");
    File.AppendAllText(filename, "Player score,Dealer score,Hit win,Hit tie,Hit lost,Hold win,Hold tie,Hold lost,Points" + Environment.NewLine);
    foreach (var write in maker.Models) {
      if (write == null) {
        continue;
      }
      File.AppendAllText(filename, write + Environment.NewLine);
    }
    Console.WriteLine(DateTime.Now + " - wrote to file");
  }
}
