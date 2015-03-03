using System.Collections.Generic;
using System.Linq;

namespace battleships
{
	public class AiTester
	{
		private readonly Settings settings;

		public AiTester(Settings settings)
		{
			this.settings = settings;
		}

		public IEnumerable<Game> TestSingleAi(Ai ai, IEnumerable<Game> games )
		{
            // —ейчас у AiTester две ответственности - запускать серию игр и накапливать результаты. 
            // ѕредлагаю избавитьс€ от второй - возвращать массив результатов игр, AiTestReporter сможет сам собрать нужную статистику.
            // Ќа RunGameToEnd можно возвращать сам результат игры
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
			}
		    return endedGames;
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