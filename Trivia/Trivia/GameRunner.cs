using System;
using System.Collections.Generic;

namespace Trivia
{
    public static class GameRunner
    {
        public static void Main(string[] args)
        {
            MakeGame(new List<string>
            {
                "Cat", "Test", "AZ", "J"
            }, 89);
        }

        private static void MakeGame(List<string> players, int seed)
        {
            Random rng = new Random(seed);
            int DiceRoll() => rng.Next(5) + 1;

            bool isTechno = false;
            InputUtilities.AskQuestion("Replace Rock questions by Techno questions ?", new Dictionary<string, Action>
            {
                {"yes", () => isTechno = true},
                {"no", () => isTechno = false}
            });

            Game aGame = new Game(isTechno,rng);
            aGame.AskGoldNumberToWin();
            aGame.Add(players);
    

            if (!aGame.IsPlayable())
            {
                Console.WriteLine("Can't start Game");
                return;
            }

            do
            {
                aGame.StartTurnText();
                if (!aGame.AskIfPlayerWantToLeaveGame())
                {
                    aGame.TryRoll(DiceRoll());
                    if (!aGame.AskForJokerUse())
                        InputUtilities.AskSuccess(rng.NextDouble() > .5f, aGame.CorrectAnswer, aGame.WrongAnswer);
                }
                else if (!aGame.IsPlayable())
                {
                    Console.WriteLine("Game Can't be played anymore");
                    return;
                }

                aGame.SelectNextPlayer();
            } while (!aGame.IsGameOver);

            aGame.DisplayLeaderboard();
        }
    }
}