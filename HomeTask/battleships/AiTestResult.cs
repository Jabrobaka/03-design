using System;
using System.Collections.Generic;

namespace battleships
{
    public class AiTestResult
    {
        public int BadShots { get; set; }
        public int Crashes { get; set; }
        public int GamesPlayed { get; set; }
        public List<int> Shots { get; set; }
        public string AiName { get; set; }

        public AiTestResult(string aiName)
        {
            AiName = aiName;
            Shots = new List<int>();
        }

        public AiTestResult(int badShots, int crashes, int gamesPlayed, List<int> shots, string aiName)
        {
            BadShots = badShots;
            Crashes = crashes;
            GamesPlayed = gamesPlayed;
            Shots = shots;
            AiName = aiName;
        }
    }
}
