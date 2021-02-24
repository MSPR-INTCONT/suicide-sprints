using System;
using System.Collections.Generic;

namespace Trivia
{
    public class Config
    {
        public int _coinsToWin;
        public int _seed;
        public Config()
        {
            AskGoldNumberToWin();
            // AskForSeed();
        }
        
        public void AskGoldNumberToWin()
        {
              _coinsToWin = InputUtilities.AskForNumber("How much coins to win ?", 6);
        }

        public void AskForSeed()
        {
            _seed = InputUtilities.AskForNumber("What is the seed ?", Int32.MinValue);
        }
    }
}