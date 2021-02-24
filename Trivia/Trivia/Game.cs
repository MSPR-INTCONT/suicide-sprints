using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        private int PlayersCount => _players.Count;
        public bool DidPlayerWin => _purses[_currentPlayerIndex] == 6;

        private readonly List<string> _players = new List<string>();

        private readonly List<int> _places = new List<int>();
        private readonly List<int> _purses = new List<int>();
        private readonly List<bool> _inPenaltyBox = new List<bool>();
    
        private readonly List<Queue<string>> _questionsCategory = new List<Queue<string>>();

        private int _currentPlayerIndex;
        private bool _isGettingOutOfPenaltyBox;

        public Game(bool useTechnoQuestion)
        {
            List<string> categories = new List<string>
            {
                "Pop", "Science", "Sports", useTechnoQuestion ? "Techno" : "Rock"
            };
            
            for (int i = 0; i < 4; i++)
                _questionsCategory.Add(new Queue<string>());
            for (int i = 0; i < 50; i++)
            {
                _questionsCategory[0].Enqueue(CreateQuestion(i,categories[0]));
                _questionsCategory[1].Enqueue(CreateQuestion(i,categories[1]));
                _questionsCategory[2].Enqueue(CreateQuestion(i,categories[2]));
                _questionsCategory[3].Enqueue(CreateQuestion(i,categories[3]));
            }
        }

        private string CreateQuestion(int index, string questionType) => $"{questionType} Question {index}";

        public bool IsPlayable() => PlayersCount >= 2 && PlayersCount <= 6;

        public void Add(List<string> playerNames)
        {
            foreach (string player in playerNames)
            {
                _places.Insert(PlayersCount, 0);
                _purses.Insert(PlayersCount, 0);
                _inPenaltyBox.Insert(PlayersCount, false);
                _players.Add(player);
                NewPlayerAddedText(player);
            }
        }

        public void Roll(int roll)
        {
            StartTurnText(roll);

        public void TryRoll(int roll)
        {
            if (!CanPlay(roll)) return;
            Roll(roll);
            NewQuestionText();
        }

        private bool CanPlay(int roll)
        {
            if (CurrentPlayer.InPenaltyBox)
            {
                CurrentPlayer.InPenaltyBox = roll % 2 == 0;
                GettingOutOfPenaltyText(CurrentPlayer.InPenaltyBox);
                return !CurrentPlayer.InPenaltyBox;
            }

            return true;
        }

        private void Roll(int roll)
        {
            RollText(roll);
            CurrentPlayer.Place = (CurrentPlayer.Place + roll) % 12;
        } 

        public bool AskIfPlayerWantToLeaveGame()
        {
            bool quitGame = false;
            InputUtilities.AskQuestion("Do you want to quit game ?", new Dictionary<string, Action>
            {
                {
                    "yes", () =>
                    {
                        quitGame = true;
                        _players.Remove(CurrentPlayer);
                    }
                },
                {"no", null}
            });
            return quitGame;
        }

        public bool AskForJokerUse()
        {
            if (!CurrentPlayer.HasJoker) return false;

            bool useJoker = false;
            InputUtilities.AskQuestion("Do you want to use a joker ?", new Dictionary<string, Action>
            {
                {
                    "yes", () =>
                    {
                        useJoker = true;
                        CurrentPlayer.HasJoker = false;
                    }
                },
                {"no", null}
            });
            return useJoker;
        }

        public void CorrectAnswer()
        {
            if (!_inPenaltyBox[_currentPlayerIndex] || _isGettingOutOfPenaltyBox)
            {
                _purses[_currentPlayerIndex]++;
                CorrectAnswerText();
            }

            SelectNextPlayer();
        }

        public void WrongAnswer()
        {
            WrongAnswerText();

            _inPenaltyBox[_currentPlayerIndex] = true;

            SelectNextPlayer();
        }

        private void SelectNextPlayer() => _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

        private void NewPlayerAddedText(string player) =>
            Console.WriteLine($"{player} was added\r\n" +
                              $"They are player number {PlayersCount}");

        private void StartTurnText() =>
            Console.WriteLine($"{CurrentPlayer} is the current player");

        private void RollText(int roll) => Console.WriteLine($"They have rolled a {roll}");

        private void GettingOutOfPenaltyText(bool inPenaltyBox) =>
            Console.WriteLine($"{CurrentPlayer} is {(inPenaltyBox ? "not" : "")} getting out of the penalty box");

        private void NewQuestionText() =>
            Console.WriteLine($"{CurrentPlayer}'s new location is {CurrentPlayer.Place}\r\n" +
                              $"The category is {CurrentCategoryName}\r\n" +
                              $"{CurrentCategoryQueue.Dequeue()}");

        private void CorrectAnswerText() =>
            Console.WriteLine("Answer was correct!!!!\r\n" +
                              $"{CurrentPlayer} now has {CurrentPlayer.Coins} Gold Coins.");

        private void WrongAnswerText() =>
            Console.WriteLine("Question was incorrectly answered\r\n" +
                              $"{CurrentPlayer} was sent to the penalty box");
    }
}