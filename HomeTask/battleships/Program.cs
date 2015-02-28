using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
                var testReporter = new AiTestReporter(settings);
                var testResult = TestAi(settings, aiPath);
		        testReporter.WriteTotal(testResult);
		    }
		    else
		        Console.WriteLine("No AI exe-file " + aiPath);
		}

	    private static AiTestResult TestAi(Settings settings, string aiPath)
	    {
	        var tester = new AiTester(settings);
	        var aiFactory = new AiFactory(settings);
	        var visualiser = new GameVisualizer();
	        var ai = aiFactory.CreateAi(aiPath);
	        var gameGenerator = new GameGenerator(settings);

            // 3 события, 2 абстракции + один экшн - но по факту ответственность одна - визуализация работы игры
            // Предлагаю вообще отвязаться от событийной модели + сделать одну абстракцию для визуализации результатов

	        gameGenerator.AddGameStepPerformedHandler(visualiser.Visualize);
	        if (settings.Verbose)
	        {
	            gameGenerator.AddGameEndedHandler(new GameVerboseWriter().Write);
	        }
	        if (settings.Interactive)
	        {
	            gameGenerator.AddAiCrashHandler(game =>
	            {
	                Console.WriteLine(game.LastError.Message);
	                Console.ReadKey();
	            });
	        }
	        var testResult = tester.TestSingleAi(ai, gameGenerator);
	        return testResult;
	    }
	}
}