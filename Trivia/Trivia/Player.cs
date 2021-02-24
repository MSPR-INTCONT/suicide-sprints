using System.Collections.Generic;

namespace Trivia
{
    public class Player
    {
        private readonly string _name;

        public Player(string name)
        {
            _name = name;
            Place = 0;
            Coins = 0;
            WinStreak = 1;
            InPenaltyBox = false;
            HasJoker = true;
        }

        public int Place { get; set; }

        public int Coins { get; set; }

        public int WinStreak { get; set; }
        
        public bool InPenaltyBox { get; set; }

        public bool HasJoker { get; set; }
        
        public override string ToString() => _name;
    }
}