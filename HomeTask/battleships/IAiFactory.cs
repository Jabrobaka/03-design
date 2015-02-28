namespace battleships
{
    public interface IAiFactory
    {
        // Чаще фабричные методы называют Create или Build
        IAi Get(string exePath);
    }
}
