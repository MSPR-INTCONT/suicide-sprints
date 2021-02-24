using System.Collections.Generic;

namespace Trivia
{
    public class Player
    {
        private readonly string _name;
        private int _amountOfTimeInPrison;
        public Player(string name)
        {
            _name = name;
            _inPenaltyBox = false;
            _amountOfTimeInPrison = 0;
            Place = 0;
            Coins = 0;
            WinStreak = 1;
            HasJoker = true;
            
        }

        public int Place { get; set; }

        public int Coins { get; set; }

        public int WinStreak { get; set; }

        private bool _inPenaltyBox;
        public bool InPenaltyBox
        {
            get => _inPenaltyBox;
            set
            {
                if (_inPenaltyBox == false && value)
                {
                    _amountOfTimeInPrison++;
                }
                _inPenaltyBox = value;
            } }

        public bool HasJoker { get; set; }

        public int AmountOfTimeInPrison => _amountOfTimeInPrison;

        
        public override string ToString() => _name;
    }
}