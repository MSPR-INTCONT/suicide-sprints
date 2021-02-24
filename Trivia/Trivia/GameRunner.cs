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
                if (!aGame.IsPlayable())
                {
                    Console.WriteLine("Game Can't be played anymore");
                    return;
                }
                aGame.StartTurn();
                if (!aGame.AskIfPlayerWantToLeaveGame())
                {
                    aGame.TryRoll(rand.Next(5) + 1);
                    if (!aGame.AskForJokerUse())
                    {
                        if (rand.Next(9) == 7)
                            aGame.WrongAnswer();
                        else
                            aGame.CorrectAnswer();
                    }
                }

                aGame.SelectNextPlayer();
                
            } while (!aGame.HaveAWinner);
        }
    }
}