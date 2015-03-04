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
		    if (!File.Exists(aiPath))
                Console.WriteLine("No AI exe-file " + aiPath);

		    var visualiser = new GameVisualizer(settings);
		    var tester = new AiTester(settings, visualiser);
            var mapGenerator = new MapGenerator(settings, new Random(settings.RandomSeed));

		    var processMonitor = 
                new ProcessMonitor(TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount), settings.MemoryLimit);           
            var ai = new Ai(aiPath);
            ai.ProcessCreated += processMonitor.Register;

		    var games = Enumerable
		        .Range(0, settings.GamesCount)
		        .Select(i => mapGenerator.GenerateMap())
		        .Select(map => new Game(map, ai));

            tester.TestSingleAi(ai, games);
		}
	}
}