namespace battleships
{
    public interface IGameFactory
    {
        IGame Get(IAi ai);
    }
}
