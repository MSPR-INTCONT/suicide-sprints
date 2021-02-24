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

            aGame.Run();
        }
    }
}