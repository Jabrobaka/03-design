namespace battleships
{
    public interface IAiFactory
    {
        IAi Create(string exePath);
    }
}
