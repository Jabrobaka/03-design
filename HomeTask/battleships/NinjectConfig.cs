using System;
using Ninject;
using Ninject.Modules;

namespace battleships
{
    class NinjectConfig : NinjectModule
    {
        public override void Load()
        {
            Bind<Settings>().ToSelf().InSingletonScope().WithConstructorArgument("settings.txt");
            var settings = Kernel.Get<Settings>();

            Bind<IGameVisualizer>().To<GameVisualizer>();
            Bind<IMapGenerator>().To<MapGenerator>()
                //todo: эта строчка лишняя, при попытке подтянуть Settings контейнер и так найдет нужные настройки
                .WithConstructorArgument("settings", settings)
                .WithConstructorArgument("random", new Random(settings.RandomSeed));
            Bind<IProcessMonitor>().To<ProcessMonitor>()
                .WithConstructorArgument("timeLimit", TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount))
                .WithConstructorArgument("memoryLimit", (long) settings.MemoryLimit);
            Bind<IAiFactory>().To<AiFactory>();
            Bind<IGameFactory>().To<GameFactory>();
        }
    }
}
