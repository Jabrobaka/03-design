using System;
using System.Collections.Generic;

namespace battleships
{
    public class GameGenerator
    {
        private MapGenerator mapGenerator;
        private int gamesCount;

        public GameGenerator(Settings settings)
        {
            mapGenerator = new MapGenerator(settings, new Random(settings.RandomSeed));
            gamesCount = settings.GamesCount;
        }

        public IEnumerable<Game> GenerateGames(Ai ai)
        {
            var map = mapGenerator.GenerateMap();
            for (int i = 0; i < gamesCount; i++)
            {
                yield return new Game(map, ai);   
            }
        } 
    }
}
