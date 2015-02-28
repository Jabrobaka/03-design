namespace battleships
{
    public interface IGameFactory
    {
        // Чаще фабричные методы называют Create или Build
        IGame Get(IAi ai);
    }
}
