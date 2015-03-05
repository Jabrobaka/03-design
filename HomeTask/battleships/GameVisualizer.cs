using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace battleships
{
	public class GameVisualizer
	{
        private readonly Settings settings;
        private static readonly Logger resultsLog = LogManager.GetLogger("results");

        public GameVisualizer(Settings settings)
        {
            this.settings = settings;
        }

        public void WriteGameResult(Game game, int gameIndex)
        {
            if (!settings.Verbose)
                return;

            Console.WriteLine(
                "Game #{3,4}: Turns {0,4}, BadShots {1}{2}",
                game.TurnsCount, game.BadShots, game.AiCrashed ? ", Crashed" : "", gameIndex);
        }

        public void WriteTotal(string aiName, IEnumerable<Game> totalGames)
        {
            var games = totalGames.ToList();
            var shots = games.Where(game => !game.AiCrashed).Select(game => game.TurnsCount).ToList();
            var crashes = games.Count(game => game.AiCrashed);
            var badshots = games.Sum(game => game.BadShots);
            WriteTotal(aiName, shots, crashes, badshots, games.Count);
        }        

        private void WriteTotal(string aiName, List<int> shots, int crashes, int badShots, int gamesPlayed)
        {
            if (shots.Count == 0) shots.Add(1000 * 1000);
            shots.Sort();
            var median = shots.Count % 2 == 1 ? shots[shots.Count / 2] : (shots[shots.Count / 2] + shots[(shots.Count + 1) / 2]) / 2;
            var mean = shots.Average();
            var sigma = Math.Sqrt(shots.Average(s => (s - mean) * (s - mean)));
            var badFraction = (100.0 * badShots) / shots.Sum();
            var crashPenalty = 100.0 * crashes / settings.CrashLimit;
            var efficiencyScore = 100.0 * (settings.Width * settings.Height - mean) / (settings.Width * settings.Height);
            var score = efficiencyScore - crashPenalty - badFraction;
            var headers = FormatTableRow(new object[] { "AiName", "Mean", "Sigma", "Median", "Crashes", "Bad%", "Games", "Score" });
            var message = FormatTableRow(new object[] { aiName, mean, sigma, median, crashes, badFraction, gamesPlayed, score });
            resultsLog.Info(message);
            Console.WriteLine();
            Console.WriteLine("Score statistics");
            Console.WriteLine("================");
            Console.WriteLine(headers);
            Console.WriteLine(message);
        }

        private string FormatTableRow(object[] values)
        {
            return FormatValue(values[0], 15)
                + string.Join(" ", values.Skip(1).Select(v => FormatValue(v, 7)));
        }

        private static string FormatValue(object v, int width)
        {
            return v.ToString().Replace("\t", " ").PadRight(width).Substring(0, width);
        }

		public void VisualizeStep(Game game)
		{
		    if (!settings.Interactive)
		        return;

			Console.Clear();
			Console.WriteLine(MapToString(game));
			Console.WriteLine("Turn: {0}", game.TurnsCount);
			Console.WriteLine("Last target: {0}", game.LastTarget);
			if (game.BadShots > 0)
				Console.WriteLine("Bad shots: " + game.BadShots);
			if (game.IsOver())
				Console.WriteLine("Game is over");
            if (game.AiCrashed)
                Console.WriteLine(game.LastError.Message);
            Console.ReadKey();

		}       

		private string MapToString(Game game)
		{
			var map = game.Map;
			var sb = new StringBuilder();
			for (var y = 0; y < map.Height; y++)
			{
				for (var x = 0; x < map.Width; x++)
					sb.Append(GetSymbol(map[new Vector(x, y)]));
				sb.AppendLine();
			}
			return sb.ToString();
		}

		private string GetSymbol(MapCell cell)
		{
			switch (cell)
			{
				case MapCell.Empty:
					return " ";
				case MapCell.Miss:
					return "*";
				case MapCell.Ship:
					return "O";
				case MapCell.DeadOrWoundedShip:
					return "X";
				default:
					throw new Exception(cell.ToString());
			}
		}
	}
}