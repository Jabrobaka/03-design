using System;

namespace battleships
{
    class AiFactory
    {
        private ProcessMonitor processMonitor;

        public AiFactory(Settings settings)
        {
            // 1. Здесь присутствует неявная зависимость от ProcessMonitor, а Dependency elimination principle говорит нам избавляться от них.
            // 2. Кроме того, это очень жесткая зависимость. Тот, кто использует класс, может сам решить - 
            // хочет он замерять потребляемые процессом ресурсы (и подписаться на события) или нет.
            // Предлагаю вынести создание монитора и подписку на события на уровень выше. Ну и фабрику вообще упразднить можно.
            processMonitor = new ProcessMonitor(
                    TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount),
                    settings.MemoryLimit);
        }

        // Можно и просто Create, контекст названия класса помогает понять, что создается
        public Ai CreateAi(string aiExePath)
        {
            var ai = new Ai(aiExePath);
            ai.ProcessCreated += processMonitor.Register;
            return ai;
        }
    }
}
