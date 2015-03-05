using System.Collections.Generic;
using System.Linq;

namespace battleships
{
	public class AiTester
	{
		private readonly Settings settings;
        // Окей, пусть так - если по-другому не влазит
	    private readonly GameVisualizer gameVisualizer;

	    public AiTester(Settings settings, GameVisualizer gameVisualizer)
		{
		    this.settings = settings;
		    this.gameVisualizer = gameVisualizer;
		}

	    public void TestSingleAi(Ai ai, IEnumerable<Game> games)
		{
		    var crashes = 0;
		    var endedGames = new List<Game>();
            foreach (var game in games)
			{
				RunGameToEnd(game);
				if (game.AiCrashed)
				{
				    crashes++;
					if (crashes > settings.CrashLimit) break;
				    ai.Restart();
				}
                endedGames.Add(game);
                gameVisualizer.WriteGameResult(game, endedGames.Count);
			}
            gameVisualizer.WriteTotal(ai.Name, endedGames);
		}

	    private void RunGameToEnd(Game game)
		{
			while (!game.IsOver())
			{
				game.MakeStep();
                gameVisualizer.VisualizeStep(game);
			}
		}	    
	}
}