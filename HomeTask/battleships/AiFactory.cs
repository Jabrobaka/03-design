namespace battleships
{
    public class AiFactory : IAiFactory
    {
        private readonly IProcessMonitor monitor;

        public AiFactory(IProcessMonitor monitor)
        {
            this.monitor = monitor;
        }

        public IAi Create(string exePath)
        {
            return new Ai(exePath, monitor);
        }
    }
}
