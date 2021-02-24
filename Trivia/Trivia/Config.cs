using System;
using System.Collections.Generic;

namespace Trivia
{
    public class Config
    {
        public int _coinsToWin;
        public bool _isTechno;
        public int _seed;
        public Config()
        {
            AskGoldNumberToWin();
            AskForTechnoOrRock();
            // AskForSeed();
        }
        
        public void AskGoldNumberToWin()
        {
              _coinsToWin = InputUtilities.AskForNumber("How much coins to win ?", 6);
        }

        public void AskForTechnoOrRock()
        {
            InputUtilities.AskQuestion("Replace Rock questions by Techno questions ?", new Dictionary<string, Action>
            {
                {"yes", () => _isTechno = true},
                {"no", () => _isTechno = false}
            });
        }

        public void AskForSeed()
        {
            _seed = InputUtilities.AskForNumber("What is the seed ?", Int32.MinValue);
        }
    }
}