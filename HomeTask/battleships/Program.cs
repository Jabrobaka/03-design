using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace battleships
{
	public class Program
	{
		private static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: {0} <ai.exe>", Process.GetCurrentProcess().ProcessName);
				return;
			}
			var aiPath = args[0];
			var settings = new Settings("settings.txt");
		    if (File.Exists(aiPath))
		    {
                var tester = new AiTester(settings);
                var visualiser = new GameVisualizer(settings);

                var processMonitor = new ProcessMonitor(
                        TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount),
                        settings.MemoryLimit);
                var ai = new Ai(aiPath);
                ai.ProcessCreated += processMonitor.Register;

                var mapGenerator = new MapGenerator(settings, new Random(settings.RandomSeed));
                var games = Enumerable.Range(0, settings.GamesCount)
                    .Select(i => mapGenerator.GenerateMap())
                    .Select(map =>
                    {
                        var game = new Game(map, ai);
                        if (settings.Interactive)
                        {
                            game.GameStepPerformed += visualiser.VisualizeStep;
                        }

                        return game;
                    });

                var endedGames = tester.TestSingleAi(ai, games);
                visualiser.WriteTotal(ai.Name, endedGames);
		    }
		    else
		        Console.WriteLine("No AI exe-file " + aiPath);
		}
	}
}