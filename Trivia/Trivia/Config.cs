using System;
using System.Collections.Generic;

namespace Trivia
{
    public class Config
    {
        public int CoinsToWin;
        public int Seed;
        public List<string> Players;
        public bool UseTechno;
        public int MaxAmountOfPrisoners;

        public Config(List<string> playerNames)
        {
            Players = playerNames;
            AskGoldNumberToWin();
            AskUseTechno();
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
        
        public void AskUseTechno()
        {
            bool useTechno = false;
            InputUtilities.AskQuestion("Do you want to replace Rock by Techno ?", new Dictionary<string, Action>
            {
                {"yes", () => useTechno = true},
                {"no", () => useTechno = false},
            });
            UseTechno = useTechno;
        }

        public void AskForSeed()
        {
            Seed = InputUtilities.AskForNumber("What is the seed ?", Int32.MinValue);
        }
    }
}