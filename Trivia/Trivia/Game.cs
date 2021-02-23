using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        private int PlayersCount => _players.Count;
        private bool DidPlayerWin => _purses[_currentPlayerIndex] != 6;

        private readonly List<string> _players = new List<string>();

        private readonly List<int> _places = new List<int>();
        private readonly List<int> _purses = new List<int>();
        private readonly List<bool> _inPenaltyBox = new List<bool>();
        
        private readonly List<Queue<string>> _questionsCategory = new List<Queue<string>>();

        private int _currentPlayerIndex;
        private bool _isGettingOutOfPenaltyBox;

        public Game(bool useTechnoQuestion)
        {
            for (int i = 0; i < 4; i++)
                _questionsCategory.Add(new Queue<string>());
            for (int i = 0; i < 50; i++)
            {
                _questionsCategory[0].Enqueue(CreatePopQuestion(i));
                _questionsCategory[1].Enqueue(CreateScienceQuestion(i));
                _questionsCategory[2].Enqueue(CreateSportsQuestion(i));
                if (useTechnoQuestion)
                {
                    _questionsCategory[3].Enqueue(CreateTechnoQuestion(i));
                }
                else
                {
                    _questionsCategory[3].Enqueue(CreateRockQuestion(i));
                }
            }
        }

        private string CreateRockQuestion(int index) => "Rock Question " + index;
        private string CreateTechnoQuestion(int index) => "Techno Question " + index;
        private string CreatePopQuestion(int index) => "Pop Question " + index;
        private string CreateSportsQuestion(int index) => "Sports Question " + index;
        private string CreateScienceQuestion(int index) => "Science Question " + index;
        
         public bool IsPlayable()
         {
             int playersCount = _players.Count;
             return (playersCount >= 2 && playersCount <= 6);
         }
         
        public void Add(List<string> playerNames)
        {
            foreach (string player in playerNames)
            {
                _places.Insert(PlayersCount,0);
                _purses.Insert(PlayersCount,0);
                _inPenaltyBox.Insert(PlayersCount,false);
                _players.Add(player);
                Console.WriteLine(player + " was added");
                Console.WriteLine("They are player number " + _players.Count);
            }
        }

        public void Roll(int roll)
        {
            StartTurnText(roll);

            if (_inPenaltyBox[_currentPlayerIndex])
            {
                RollWhenInPenaltyBox(roll);

                if (_isGettingOutOfPenaltyBox)
                    NewQuestionText();
            }
            else
            {
                RollWhenNotInPenaltyBox(roll);

                NewQuestionText();
            }
        }

        private void RollWhenInPenaltyBox(int roll)
        {
            _isGettingOutOfPenaltyBox = roll % 2 != 0;
            if (!_isGettingOutOfPenaltyBox)
            {
                NotGettingOutOfPenalty();
                return;
            }

            GettingOutOfPenaltyText();
            RollWhenNotInPenaltyBox(roll);
        }

        private void RollWhenNotInPenaltyBox(int roll)
        {
            _places[_currentPlayerIndex] = _places[_currentPlayerIndex] + roll;
            if (_places[_currentPlayerIndex] > 11) _places[_currentPlayerIndex] = _places[_currentPlayerIndex] - 12;
        }

        private int CurrentCategory() => _places[_currentPlayerIndex] % 4;

        public bool WasCorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayerIndex]  && !_isGettingOutOfPenaltyBox)
            {
                SelectNextPlayer();
                
                return true;
            }

            _purses[_currentPlayerIndex]++;
            
            CorrectAnswerText();

            SelectNextPlayer();

            return DidPlayerWin;
        }

        public void WrongAnswer()
        {
            WrongAnswerText();

            _inPenaltyBox[_currentPlayerIndex] = true;
            
            SelectNextPlayer();
        }

        private void SelectNextPlayer() => _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

        
        
        private void StartTurnText(int roll)
        {
            Console.WriteLine(_players[_currentPlayerIndex] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);
        }
        
        
        private void NotGettingOutOfPenalty() =>
            Console.WriteLine(_players[_currentPlayerIndex] + " is not getting out of the penalty box");

        private void GettingOutOfPenaltyText() =>
            Console.WriteLine(_players[_currentPlayerIndex] + " is getting out of the penalty box");

        private void NewQuestionText()
        {
            Console.WriteLine(_players[_currentPlayerIndex]
                              + "'s new location is "
                              + _places[_currentPlayerIndex]);
            Console.WriteLine("The category is " + CurrentCategory());
            Console.WriteLine(_questionsCategory[CurrentCategory()].Dequeue());
        }

        private void CorrectAnswerText()
        {
            Console.WriteLine("Answer was corrent!!!!");
            Console.WriteLine(_players[_currentPlayerIndex]
                              + " now has "
                              + _purses[_currentPlayerIndex]
                              + " Gold Coins.");
        }

        private void WrongAnswerText()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(_players[_currentPlayerIndex] + " was sent to the penalty box");
        }
    }
}