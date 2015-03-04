using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Ninject;
using Ninject.Extensions.Conventions;

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
		    var DIContainer = InitContainer();
			var tester = DIContainer.Get<AiTester>();
			if (File.Exists(aiPath))
				tester.TestSingleFile(aiPath);
			else
				Console.WriteLine("No AI exe-file " + aiPath);
		}

        private static IKernel InitContainer()
        {
            var kernel = new StandardKernel();
            
            kernel.Bind<Settings>().ToSelf().InSingletonScope().WithConstructorArgument("settings.txt");
            var settings = kernel.Get<Settings>();

            kernel.Bind(c =>
                c.FromThisAssembly()
                    .SelectAllClasses()
                    .BindAllInterfaces()
                    .Configure(b => b.InSingletonScope())
                    .ConfigureFor<MapGenerator>(
                        b => b.WithConstructorArgument("random", new Random(settings.RandomSeed)))
                    .ConfigureFor<ProcessMonitor>(
                        b => b.WithConstructorArgument("timeLimit", TimeSpan.FromSeconds(settings.TimeLimitSeconds*settings.GamesCount))
                              .WithConstructorArgument("memoryLimit", (long) settings.MemoryLimit)));

            return kernel;
        }
	}
}