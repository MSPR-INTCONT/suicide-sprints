using System;
using System.Collections.Generic;

namespace Trivia
{
    public static class GameRunner
    {
        private static bool _notAWinner;

        public static void Main(string[] args)
        {
            Console.WriteLine("Techno (y) : ");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            bool isTechno = keyInfo.Key == ConsoleKey.Y;
            var aGame = new Game(isTechno);
            
            aGame.Add(new List<string>()
            {
                "Cat"
            });


            if (!aGame.IsPlayable())
            {
                Console.WriteLine("Pas possible de lancer la partie");
                return;
            }
            
            var rand = new Random();

            do
            {
                aGame.Roll(rand.Next(5) + 1);

                if (rand.Next(9) == 7)
                {
                    aGame.WrongAnswer();
                    _notAWinner = true;
                }
                else
                {
                    _notAWinner = aGame.WasCorrectlyAnswered();
                }
            } while (_notAWinner);
        }
    }
}