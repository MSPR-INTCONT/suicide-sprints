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
            Game aGame = new Game(configGame, 0);
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
        }
    }
}