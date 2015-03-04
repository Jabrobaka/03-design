namespace battleships
{
    public interface IGameFactory
    {
        IGame Create(IAi ai);
    }
}
