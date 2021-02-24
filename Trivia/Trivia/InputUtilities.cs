using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public static class InputUtilities
    {
        public static void AskQuestion(string question, Dictionary<string, Action> possibleAnswers)
        {
            string answerConcat = String.Empty;             
            foreach (KeyValuePair<string, Action> possibleAnswer in possibleAnswers)                 
                answerConcat += $" ({possibleAnswer.Key})";             
                string answer;
                do
                {
                    Console.WriteLine($"Questions : {question}\r\nAccepted Answers :{answerConcat}");
                    answer = Console.ReadLine();
                } while (answer is null || !possibleAnswers.ContainsKey(answer));
                if (!(possibleAnswers[answer] is null))                
                    possibleAnswers[answer]();
        }

        public static int AskForNumber()
        {
            string answer;
            int result;
            do
            {
                Console.WriteLine("Quelle est la valeur à atteindre pour gagner ?");
                answer = Console.ReadLine();
            } while (answer is null || !(Int32.TryParse(answer, out result) && result >= 6));
            return result;
        }

        public static void AskSucces(bool succes, Action trueAction, Action falseAction)
        {
            if (succes)
            {
                trueAction();
            }
            else
            {
                falseAction();
            }
        }
    }
}