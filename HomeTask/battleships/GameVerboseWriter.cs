using System;

namespace battleships
{
    // Очень странная абстракция + внутри хранится состояние, в рамках этого задания мы хотим уйти от таких вещей
    class GameVerboseWriter
    {
        private int gamesWritten = 0;
        public void Write(Game game)
        {
            Console.WriteLine(
                "Game #{3,4}: Turns {0,4}, BadShots {1}{2}",
                game.TurnsCount, game.BadShots, game.AiCrashed ? ", Crashed" : "", gamesWritten++);
        }
    }
}
