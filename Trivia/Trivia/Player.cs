using System.Collections.Generic;

namespace Trivia
{
    public class Player
    {
        private readonly string _name;
        private int _amountOfTimeInPrison = 0;
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
        
        public bool InPenaltyBox
        {
            get => InPenaltyBox;
            set
            {
                if (InPenaltyBox == false && value)
                {
                    _amountOfTimeInPrison++;
                } 
            } }

        public bool HasJoker { get; set; }

        public int AmountOfTimeInPrison => _amountOfTimeInPrison;

        
        public override string ToString() => _name;
    }
}