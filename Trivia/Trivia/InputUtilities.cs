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

        public static void AskSuccess(bool success, Action trueAction, Action falseAction)
        {
            if (!success)
            {
                falseAction();
                return;
            }

            trueAction();
        }

        public static string AskChoices(string question, List<string> choices)
        {
            string answerConcat = String.Empty;
            foreach (string choice in choices)
                answerConcat += $" ({choice})";
            string answer;
            do
            {
                Console.WriteLine($"Questions : {question}\r\nAccepted Answers :{answerConcat}");
                answer = Console.ReadLine();
            } while (answer is null || !choices.Contains(answer));

            return answer;
        }
    }
}