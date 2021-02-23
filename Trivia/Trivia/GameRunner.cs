using System;
using System.Collections.Generic;

namespace Trivia
{
    public static class GameRunner
    {
        private static bool _notAWinner;

        public static void Main(string[] args)
        {
            var aGame = new Game(false);

            aGame.Add(new List<string>()
            {
                "Cat", "Dog", "Rat", "truc", "defef"
            });


            if (!aGame.IsPlayable())
            {
                Console.WriteLine("Pas possible");
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