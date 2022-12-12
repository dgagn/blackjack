// See https://aka.ms/new-console-template for more information

using System.Text;
using Blackjack;
using V2;

const string filename = "v2.csv";

Model?[,,] models = new Model[21,21,400];

var text = File.ReadAllText(filename, Encoding.UTF8)
    .Split("\n")
    .Skip(1)
    .Select(s => s.Trim())
    .Where(i => i.Length != 0)
    .Select(line => {
        var lines = line.Split(',');
        if (lines.Length != 9)
            throw new InvalidOperationException("Invalid CSV format");
        return new Model {
            PlayerScore = int.Parse(lines[0]),
            DealerScore = int.Parse(lines[1]),
            DeckScore = int.Parse(lines[2]),
            Hit = new ModelDecision {
                WinCount = ulong.Parse(lines[3]),
                TieCount = ulong.Parse(lines[4]),
                LostCount = ulong.Parse(lines[5]),
            },
            Hold = new ModelDecision {
                WinCount = ulong.Parse(lines[6]),
                TieCount = ulong.Parse(lines[7]),
                LostCount = ulong.Parse(lines[8])
            }
        };
    })
    .ToArray();

foreach (var line in text) {
    models[line.PlayerScore, line.DealerScore, line.DeckScore] = line;
}

var maker = new V2.V2(models);

var count = 0;

while (true) {
    var game = new Game();
    game.Deal(50);
    if (game.Player.HandValue() == 21) continue;

    var state = new ModelState(game.Player.HandValue(), game.Dealer.HandValue(), game.Deck.HandValue());
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
        File.AppendAllText(filename, "Player score,Dealer score,Deck score,Hit win,Hit tie,Hit lost,Hold win,Hold tie,Hold lost" + Environment.NewLine);
        ulong games = 0;
        ulong wins = 0;
        foreach (var write in maker.Models) {
            if (write == null) {
                continue;
            }

            games += write.Hit.GameCount + write.Hold.GameCount;
            wins += write.Hit.WinCount + write.Hold.WinCount +
                   write.Hit.TieCount + write.Hold.TieCount;
            
            File.AppendAllText(filename, write + Environment.NewLine);
        }

        const string ml = "ml.csv";
        if (!File.Exists(ml)) File.Create(ml);
        if (!File.ReadLines(ml).Any())
            File.AppendAllText(ml, "Time,Wins percentage" + Environment.NewLine);
        var value = (double)wins / games;
        File.AppendAllText(ml, $"{DateTime.Now},{value}{Environment.NewLine}");
        Console.WriteLine(DateTime.Now + $" - wrote to file {value:P}");
    }
}