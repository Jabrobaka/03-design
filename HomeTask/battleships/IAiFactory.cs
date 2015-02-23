namespace battleships
{
    public interface IAiFactory
    {
        IAi Get(string exePath);
    }
}
