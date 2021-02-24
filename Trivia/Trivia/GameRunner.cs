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
                "Cat", "Test"
            }, 89);
        }

        private static void MakeGame(List<string> players, int seed)
        {
            Config configGame = new Config();
            bool newGame = false;
            do
            {
                Game aGame = new Game(configGame);
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
                        aGame.TryRoll();
                        if (!aGame.AskForJokerUse())
                            aGame.Answer(true);
                    }
                    else if (!aGame.IsPlayable())
                    {
                        Console.WriteLine("Game Can't be played anymore");
                        return;
                    }

                    aGame.SelectNextPlayer();
                } while (!aGame.IsGameOver);

                aGame.DisplayLeaderboard();

                InputUtilities.AskQuestion("Do you want to play a new game ?", new Dictionary<string, Action>
                {
                    {"yes", () => newGame = true},
                    {"no", () => newGame = false}
                });
            } while (newGame);
        }
    }
}