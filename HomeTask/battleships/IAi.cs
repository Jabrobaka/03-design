namespace battleships
{
    public interface IAi
    {
        string Name { get; }
        Vector Init(int width, int height, int[] shipSizes);
        Vector GetNextShot(Vector lastShotTarget, ShtEffct lastShot);
        void Dispose();
    }
}