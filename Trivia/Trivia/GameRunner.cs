using System;
using System.Collections.Generic;

namespace Trivia
{
    public static class GameRunner
    {
        public static void Main(string[] args)
        {
            bool isTechno = false;
            InputUtilities.AskQuestion("Replace Rock questions by Techno questions ?", new Dictionary<string, Action>
            {
                {"yes", () => isTechno = true},
                {"no", () => isTechno = false}
            });

            Game aGame = new Game(isTechno);
            aGame.Add(new List<string>
            {
                "Cat", "Test"
            });


            if (!aGame.IsPlayable())
            {
                Console.WriteLine("Can't start Game");
                return;
            }

            Random rand = new Random();

            do
            {
                aGame.StartTurn();
                if (!aGame.AskIfPlayerWantToLeaveGame())
                {
                    aGame.TryRoll(InputUtilities.DiceRoll());
                    if (!aGame.AskForJokerUse())
                        InputUtilities.AskSuccess(rand.Next(9) == 7, aGame.CorrectAnswer, aGame.WrongAnswer);
                }
                else if (!aGame.IsPlayable())
                {
                    Console.WriteLine("Game Can't be played anymore");
                    return;
                }

                aGame.SelectNextPlayer();
            } while (!aGame.HaveAWinner);
        }
    }
}