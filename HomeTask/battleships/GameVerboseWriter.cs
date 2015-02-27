using System;

namespace battleships
{
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
