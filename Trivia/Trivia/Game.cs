using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        private int PlayersCount => _players.Count;
        private bool DidPlayerWin => _purses[_currentPlayer] != 6;


        private readonly List<string> _players = new List<string>();

        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];

        private readonly bool[] _inPenaltyBox = new bool[6];

        private readonly LinkedList<string> _popQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _scienceQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _sportsQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _rockQuestions = new LinkedList<string>();

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;

        public Game(bool useTechnoQuestion)
        {
            if (useTechnoQuestion)
            {
                ReplaceCategory();
            }
            for (int i = 0; i < 50; i++)
            {
                _popQuestions.AddLast(CreatePopQuestion(i));
                _scienceQuestions.AddLast(CreateScienceQuestion(i));
                _sportsQuestions.AddLast(CreateSportsQuestion(i));
                _rockQuestions.AddLast(CreateRockQuestion(i));
            }
        }

        private void ReplaceCategory()
        {
            
        }

        private string CreateRockQuestion(int index) => "Rock Question " + index;
        private string CreatePopQuestion(int index) => "Pop Question " + index;
        private string CreateSportsQuestion(int index) => "Sports Question " + index;
        private string CreateScienceQuestion(int index) => "Science Question " + index;

/*
        public bool IsPlayable()
        {
            return (HowManyPlayers() >= 2);
        }
*/

        public void Add(string playerName)
        {
            _players.Add(playerName);
            _places[PlayersCount] = 0;
            _purses[PlayersCount] = 0;
            _inPenaltyBox[PlayersCount] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + _players.Count);
        }


        public void Roll(int roll)
        {
            Console.WriteLine(_players[_currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (_inPenaltyBox[_currentPlayer])
            {
                RollWhenInPenaltyBox(roll);

                if (_isGettingOutOfPenaltyBox)
                    NewQuestion();
            }
            else
            {
                RollWhenNotInPenaltyBox(roll);

                NewQuestion();
            }
        }

        private void NewQuestion()
        {
            Console.WriteLine(_players[_currentPlayer]
                              + "'s new location is "
                              + _places[_currentPlayer]);
            Console.WriteLine("The category is " + CurrentCategory());
            AskQuestion();
        }

        private void RollWhenInPenaltyBox(int roll)
        {
            _isGettingOutOfPenaltyBox = roll % 2 != 0;
            if(!_isGettingOutOfPenaltyBox)
            {
                Console.WriteLine(_players[_currentPlayer] + " is not getting out of the penalty box");
                return;
            }

            Console.WriteLine(_players[_currentPlayer] + " is getting out of the penalty box");
            RollWhenNotInPenaltyBox(roll);
        }

        private void RollWhenNotInPenaltyBox(int roll)
        {
            _places[_currentPlayer] = _places[_currentPlayer] + roll;
            if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;
        }

        private void AskQuestion()
        {
            if (CurrentCategory() == "Pop")
            {
                Console.WriteLine(_popQuestions.First());
                _popQuestions.RemoveFirst();
            }

            if (CurrentCategory() == "Science")
            {
                Console.WriteLine(_scienceQuestions.First());
                _scienceQuestions.RemoveFirst();
            }

            if (CurrentCategory() == "Sports")
            {
                Console.WriteLine(_sportsQuestions.First());
                _sportsQuestions.RemoveFirst();
            }

            if (CurrentCategory() == "Rock")
            {
                Console.WriteLine(_rockQuestions.First());
                _rockQuestions.RemoveFirst();
            }
        }

        private string CurrentCategory()
        {
            if (_places[_currentPlayer] == 0) return "Pop";
            if (_places[_currentPlayer] == 4) return "Pop";
            if (_places[_currentPlayer] == 8) return "Pop";
            if (_places[_currentPlayer] == 1) return "Science";
            if (_places[_currentPlayer] == 5) return "Science";
            if (_places[_currentPlayer] == 9) return "Science";
            if (_places[_currentPlayer] == 2) return "Sports";
            if (_places[_currentPlayer] == 6) return "Sports";
            if (_places[_currentPlayer] == 10) return "Sports";
            return "Rock";
        }

        public bool WasCorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    _purses[_currentPlayer]++;
                    Console.WriteLine(_players[_currentPlayer]
                                      + " now has "
                                      + _purses[_currentPlayer]
                                      + " Gold Coins.");

                    bool winner = DidPlayerWin;
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;

                    return winner;
                }

                _currentPlayer++;
                if (_currentPlayer == _players.Count) _currentPlayer = 0;
                return true;
            }

            {
                Console.WriteLine("Answer was corrent!!!!");
                _purses[_currentPlayer]++;
                Console.WriteLine(_players[_currentPlayer]
                                  + " now has "
                                  + _purses[_currentPlayer]
                                  + " Gold Coins.");

                bool winner = DidPlayerWin;
                _currentPlayer++;
                if (_currentPlayer == _players.Count) _currentPlayer = 0;

                return winner;
            }
        }

        public void WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(_players[_currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;

            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;
        }
    }
}