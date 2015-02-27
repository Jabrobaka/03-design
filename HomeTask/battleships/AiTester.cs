namespace battleships
{
	public class AiTester
	{
		private readonly Settings settings;

		public AiTester(Settings settings)
		{
			this.settings = settings;
		}

		public AiTestResult TestSingleAi(Ai ai, GameGenerator gameGenerator)
		{
		    var testResult = new AiTestResult(ai.Name);
		    foreach (var game in gameGenerator.GenerateGames(ai))
			{
				RunGameToEnd(game);
			    testResult.GamesPlayed++;
			    testResult.BadShots += game.BadShots;
				if (game.AiCrashed)
				{
				    testResult.Crashes++;
					if (testResult.Crashes > settings.CrashLimit) break;
				    ai.Restart();
				}
				else
					testResult.Shots.Add(game.TurnsCount);
			}
		    return testResult;
		}

	    private void RunGameToEnd(Game game)
		{
			while (!game.IsOver())
			{
				game.MakeStep();
			}
		}	    
	}
}