using System.Collections.Generic;
using System.Linq;

namespace battleships
{
	public class AiTester
	{
		private readonly Settings settings;
        // ����, ����� ��� - ���� ��-������� �� ������
	    private readonly GameVisualizer gameVisualizer;

	    public AiTester(Settings settings, GameVisualizer gameVisualizer)
		{
		    this.settings = settings;
		    this.gameVisualizer = gameVisualizer;
		}

	    public IEnumerable<Game> TestSingleAi(Ai ai, IEnumerable<Game> games)
		{
            // ������ � AiTester ��� ��������������� - ��������� ����� ��� � ����������� ����������. 
            // ��������� ���������� �� ������ - ���������� ������ ����������� ���, AiTestReporter ������ ��� ������� ������ ����������.
            // �� RunGameToEnd ����� ���������� ��� ��������� ����
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