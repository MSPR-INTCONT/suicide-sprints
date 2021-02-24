using System;
using System.Collections.Generic;

namespace Trivia
{
    public class Config
    {
        public int CoinsToWin;
        public int Seed;
        public List<string> Players;
        public int MaxAmountOfPrisoners;

        public Config(List<string> playerNames)
        {
            Players = playerNames;
            AskGoldNumberToWin();
            AskMaxAmountOfPrisoners();
            // AskForSeed();
        }

        public void AskGoldNumberToWin()
        {
            CoinsToWin = InputUtilities.AskForNumber("How much coins to win ?", 6);
        }

        public void AskMaxAmountOfPrisoners()
        {
            MaxAmountOfPrisoners =
                InputUtilities.AskForNumberWithMaxValue("How much prisoners maximum ?", Players.Count - 1);
        }

        public void AskForSeed()
        {
            Seed = InputUtilities.AskForNumber("What is the seed ?", Int32.MinValue);
        }
    }
}