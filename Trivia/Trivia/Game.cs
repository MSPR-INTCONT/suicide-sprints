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
        public bool IsGameOver => _leaderboard.Count >= 3 || !IsPlayable();

        public bool HasCurrentPlayerFinished => CurrentPlayer.Coins >= _coinsToWin;

        private readonly List<string> _categories;
        private readonly List<Player> _players = new List<Player>();
        private readonly Dictionary<string, Queue<string>> _questionsCategory = new Dictionary<string, Queue<string>>();

        private int _currentPlayerIndex;
        private string _choosenCategoryName;
        private int _coinsToWin;
        private int _seed;

        private readonly Random _rng;
        private readonly List<Player> _leaderboard = new List<Player>();

        private int _questionIndex;

        private int QuestionIndex
        {
            get
            {
                _questionIndex++;
                return _questionIndex;
            }
        }

        public Game(List<string> playerNames, Config config = null)
        {
            foreach (string name in playerNames)
                _players.Add(new Player(name));

            // _rng = config is null ? new Random() : new Random(config._seed);
            _rng = new Random();
            _choosenCategoryName = null;
            _categories = new List<string>
            {
                "Pop", "Science", "Sports", config != null && config._isTechno ? "Techno" : "Rock"
            };

            foreach (string category in _categories)
                _questionsCategory.Add(category, new Queue<string>());

            _coinsToWin = config?._coinsToWin ?? 6;

            string players = String.Empty;
            foreach (Player player in _players)
                players += $" ({player}) ";

            Console.WriteLine("Game Started With Parameters:\r\n\t" +
                              $"Amount Of Coins To Win: {_coinsToWin}\r\n\t" +
                              $"Use Techno Question: {config?._isTechno}\r\n\t" +
                              $"Random Seed: {config?._seed}\r\n\t" +
                              $"Players: {players}\n\n\n");
        }

        private string CreateQuestion(int index, string questionType) => $"{questionType} Question {index}";

        private int DiceRoll() => _rng.Next(5) + 1;
        public bool IsPlayable() => PlayersCount >= 2 && PlayersCount <= 6;

        public void TryRoll()
        {
            if (!CanPlay()) return;
            Roll(DiceRoll());
            if (CurrentCategoryQueue.Count == 0)
                CurrentCategoryQueue.Enqueue(CreateQuestion(QuestionIndex, CurrentCategoryName));
            NewQuestionText();
            _choosenCategoryName = null;
        }

        private bool CanPlay()
        {
            if (CurrentPlayer.InPenaltyBox)
            {
                float chancesOfGettingOut = (1f / CurrentPlayer.AmountOfTimeInPrison) *
                                            (1 + CurrentPlayer.PercentIncreaseFromTurnSpentInPrison);
                ChancesOfGettingOutOfPenaltyText(chancesOfGettingOut);
                CurrentPlayer.InPenaltyBox = _rng.NextDouble() >= chancesOfGettingOut;
                if (CurrentPlayer.InPenaltyBox)
                    CurrentPlayer.TurnSpentInPrison++;
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
                        SelectPreviousPlayer();
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
            CurrentPlayer.Coins += CurrentPlayer.WinStreak;
            CurrentPlayer.WinStreak++;

            CorrectAnswerText();
            if (HasCurrentPlayerFinished)
            {
                WonText();
                _leaderboard.Add(CurrentPlayer);
                _players.Remove(CurrentPlayer);
                SelectPreviousPlayer();
                if (!IsPlayable())
                {
                    foreach (Player player in _players.OrderBy(player => player.Coins))
                    {
                        if (_leaderboard.Count >= 3) break;
                        _leaderboard.Add(player);
                    }
                }
            }
        }

        public void WrongAnswer()
        {
            CurrentPlayer.InPenaltyBox = true;
            CurrentPlayer.WinStreak = 1;
            WrongAnswerText();
            _choosenCategoryName = InputUtilities.AskChoices("Select question category for next player", _categories);
        }

        public void Answer(bool isCorrectAnswer)
        {
            Answer(1, isCorrectAnswer);
        }

        public void Answer(float percent, bool? isCorrectAnswer = null)
        {
            InputUtilities.AskSuccess(isCorrectAnswer ?? _rng.NextDouble() <= percent, CorrectAnswer, WrongAnswer);
        }

        public void SelectNextPlayer() => _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

        public void SelectPreviousPlayer()
        {
            _currentPlayerIndex--;
            if (_currentPlayerIndex < 0)
                _currentPlayerIndex = _players.Count - 1;
        }


        public void DisplayLeaderboard()
        {
            Console.WriteLine("\n\nLeaderboard :\n\n");
            int i = 0;
            foreach (Player leader in _leaderboard)
            {
                Console.WriteLine($"{i + 1} : {leader}");
                i++;
            }
        }

        private void WonText() => Console.WriteLine($"{CurrentPlayer} is now in leaderboard");

        public void StartTurnText() =>
            Console.WriteLine($"\n\n{CurrentPlayer} is the current player");

        private void RollText(int roll) => Console.WriteLine($"They have rolled a {roll}");

        private void ChancesOfGettingOutOfPenaltyText(float chancesOfGettingOut) =>
            Console.WriteLine(
                $"Current chances of getting out of prison are {chancesOfGettingOut * 100}%");

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