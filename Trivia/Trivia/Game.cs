using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        private int PlayersCount => _players.Count;

        public Player CurrentPlayer => _players[_currentPlayerIndex];
        private Queue<string> CurrentCategoryQueue => _questionsCategory[CurrentCategoryName];
        public string CurrentCategoryName => _choosenCategoryName ?? _categories[CurrentPlayer.Place % 4];
        public bool HaveAWinner => _players.Exists(player => player.Coins == _coinsToWin);

        private readonly List<string> _categories;
        private readonly List<Player> _players = new List<Player>();
        private readonly Dictionary<string, Queue<string>> _questionsCategory = new Dictionary<string, Queue<string>>();
        private int _currentPlayerIndex;
        private string _choosenCategoryName;
        private int _coinsToWin;


        public Game(bool useTechnoQuestion)
        {
            _choosenCategoryName = null;
            _categories = new List<string>
            {
                "Pop", "Science", "Sports", useTechnoQuestion ? "Techno" : "Rock"
            };

            foreach (string category in _categories)
            {
                Queue<string> questions = new Queue<string>();
                for (int i = 0; i < 50; i++)
                    questions.Enqueue(CreateQuestion(i, category));
                _questionsCategory.Add(category, questions);
            }

            _coinsToWin = 6;
        }
        
        private string CreateQuestion(int index, string questionType) => $"{questionType} Question {index}";

        public bool IsPlayable() => PlayersCount >= 2 && PlayersCount <= 6;

        public void Add(List<string> playerNames)
        {
            foreach (string name in playerNames)
            {
                _players.Add(new Player(name));
                NewPlayerAddedText(name);
            }
        }

        public void StartTurn()
        {
            StartTurnText();
        }

        public void TryRoll(int roll)
        {
            if (!CanPlay(roll)) return;
            Roll(roll);
            NewQuestionText();
            _choosenCategoryName = null;
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

        public void Roll(int roll)
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

        public void AskGoldNumberToWin()
        {
            _coinsToWin = InputUtilities.AskForNumber("How much coins to win ?", 6);
        }

        public void CorrectAnswer()
        {
            CurrentPlayer.Coins += CurrentPlayer.WinStreak;
            CurrentPlayer.WinStreak++;
            CorrectAnswerText();
        }

        public void WrongAnswer()
        {
            CurrentPlayer.InPenaltyBox = true;
            CurrentPlayer.WinStreak = 1;
            WrongAnswerText();
            _choosenCategoryName = InputUtilities.AskChoices("Select question category for next player", _categories);
        }


        public void SelectNextPlayer() => _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

        private void NewPlayerAddedText(string player) =>
            Console.WriteLine($"{player} was added\r\n" +
                              $"They are player number {PlayersCount}");

        private void StartTurnText() =>
            Console.WriteLine($"\n\n{CurrentPlayer} is the current player");

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