using System;

namespace battleships
{
    public interface IAi : IDisposable
    {
        string Name { get; }
        Vector Init(int width, int height, int[] shipSizes);
        Vector GetNextShot(Vector lastShotTarget, ShtEffct lastShot);
    }
}