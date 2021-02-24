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
        public bool IsGameOver => _players.Exists(player => player.Coins == _coinsToWin);

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

        public Game(Config config = null, int amountOfQuestionToGenerate = 50)
        {
            _rng = new Random(config?._seed ?? 0);
            _choosenCategoryName = null;
            _categories = new List<string>
            {
                "Pop", "Science", "Sports", config != null && config._isTechno ? "Techno" : "Rock"
            };

            foreach (string category in _categories)
            {
                Queue<string> questions = new Queue<string>();
                for (int i = 0; i < amountOfQuestionToGenerate; i++)
                    questions.Enqueue(CreateQuestion(QuestionIndex, category));
                _questionsCategory.Add(category, questions);
            }

            _coinsToWin = config?._coinsToWin ?? 6;
        }
        
        private string CreateQuestion(int index, string questionType) => $"{questionType} Question {index}";

        private int DiceRoll() => _rng.Next(5) + 1;
        public bool IsPlayable() => PlayersCount >= 2 && PlayersCount <= 6;

        public void Add(List<string> playerNames)
        {
            foreach (string name in playerNames)
            {
                _players.Add(new Player(name));
                NewPlayerAddedText(name);
            }
        }

        public void TryRoll()
        {
            if (!CanPlay()) return;
            Roll(DiceRoll());
            if(CurrentCategoryQueue.Count == 0)
                CurrentCategoryQueue.Enqueue(CreateQuestion(QuestionIndex, CurrentCategoryName));
            NewQuestionText();
            CurrentCategoryQueue.Dequeue();         
            _choosenCategoryName = null;
        }

        private bool CanPlay()
        {
            if (CurrentPlayer.InPenaltyBox)
            {
                ChancesOfGettingOutOfPenaltyText();
                CurrentPlayer.InPenaltyBox = _rng.NextDouble() >= 1f / CurrentPlayer.AmountOfTimeInPrison;
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

        public void Answer(bool isCorrectAnswer)
        {
            Answer(1, isCorrectAnswer);
        }

        public void Answer(float percent, bool? isCorrectAnswer = null)
        {
            InputUtilities.AskSuccess(isCorrectAnswer ?? _rng.NextDouble() <= percent, CorrectAnswer, WrongAnswer);
        }
        
        public void SelectNextPlayer() => _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

        private void NewPlayerAddedText(string player) =>
            Console.WriteLine($"{player} was added\r\n" +
                              $"They are player number {PlayersCount}");

        public void StartTurnText() =>
            Console.WriteLine($"\n\n{CurrentPlayer} is the current player");

        private void RollText(int roll) => Console.WriteLine($"They have rolled a {roll}");

        private void ChancesOfGettingOutOfPenaltyText() =>
            Console.WriteLine($"Current chances of getting out of prison are 1 / {CurrentPlayer.AmountOfTimeInPrison}");

        private void GettingOutOfPenaltyText(bool inPenaltyBox) =>
            Console.WriteLine($"{CurrentPlayer} is {(inPenaltyBox ? "not" : "")} getting out of the penalty box");

        private void NewQuestionText() =>
            Console.WriteLine($"{CurrentPlayer}'s new location is {CurrentPlayer.Place}\r\n" +
                              $"The category is {CurrentCategoryName}\r\n");

        private void CorrectAnswerText() =>
            Console.WriteLine("Answer was correct!!!!\r\n" +
                              $"{CurrentPlayer} now has {CurrentPlayer.Coins} Gold Coins.");

        private void WrongAnswerText() =>
            Console.WriteLine("Question was incorrectly answered\r\n" +
                              $"{CurrentPlayer} was sent to the penalty box");
    }
}