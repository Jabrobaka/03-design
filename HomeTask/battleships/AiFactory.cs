using System;

namespace battleships
{
    class AiFactory
    {
        private ProcessMonitor processMonitor;

        public AiFactory(Settings settings)
        {
            processMonitor = new ProcessMonitor(
                    TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount),
                    settings.MemoryLimit);
        }

        public Ai CreateAi(string aiExePath)
        {
            var ai = new Ai(aiExePath);
            ai.ProcessCreated += processMonitor.Register;
            return ai;
        }
    }
}
