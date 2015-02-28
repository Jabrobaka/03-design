using System;
using System.Collections.Generic;

namespace battleships
{
    public class GameGenerator
    {
        private MapGenerator mapGenerator;
        private int gamesCount;
        private List<Action<Game>> gameEndedHandlers;
        private List<Action<Game>> gameStepHandlers;
        private List<Action<Game>> gameAiCrashHandlers;

        public GameGenerator(Settings settings)
        {
            mapGenerator = new MapGenerator(settings, new Random(settings.RandomSeed));
            gamesCount = settings.GamesCount;
            gameEndedHandlers = new List<Action<Game>>();
            gameStepHandlers = new List<Action<Game>>();
            gameAiCrashHandlers = new List<Action<Game>>();
        }

        public IEnumerable<Game> GenerateGames(Ai ai)
        {
            // Будет создано gamesCount игр на одинаковых картах. Багулечка.
            var map = mapGenerator.GenerateMap();
            // Без событий абстракция для генерации нескольких игр не нужна. Можно обойтись Enumerable.Range или своим Extension'ом
            for (int i = 0; i < gamesCount; i++)
            {
                var game = new Game(map, ai);
                gameEndedHandlers.ForEach(action => game.GameEnded += action);
                gameStepHandlers.ForEach(action => game.GameStepPerformed += action);
                gameAiCrashHandlers.ForEach(action => game.AiCrash += action);
                yield return game;
            }
        }

        public void AddGameEndedHandler(Action<Game> action)
        {
            gameEndedHandlers.Add(action);
        }

        public void AddGameStepPerformedHandler(Action<Game> action)
        {
            gameStepHandlers.Add(action);
        }

        public void AddAiCrashHandler(Action<Game> action)
        {
            gameAiCrashHandlers.Add(action);
        }
    }
}
