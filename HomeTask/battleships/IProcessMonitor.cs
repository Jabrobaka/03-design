using System.Diagnostics;

namespace battleships
{
    public interface IProcessMonitor
    {
        void Register(Process process);
    }
}