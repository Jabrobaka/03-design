namespace battleships
{
    public class GameFactory : IGameFactory
    {
        private readonly IMapGenerator mapGenerator;

        public GameFactory(IMapGenerator mapGenerator)
        {
            this.mapGenerator = mapGenerator;
        }

        public IGame Create(IAi ai)
        {
            return new Game(mapGenerator.GenerateMap(), ai);
        }
    }
}
