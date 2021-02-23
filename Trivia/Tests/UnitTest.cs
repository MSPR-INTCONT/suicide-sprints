using System;
using Xunit;
using Trivia;
using System.Collections.Generic;

namespace Tests
{
    public class UnitTest
    {
     /*   [Fact]
        public void Roll_NotInPenaltyBox_Test()
        {
            Game game = new Game(false);
        }

        [Fact]
        public void Roll_InPenaltyBox_Test()
        {
            Game game = new Game(false);
        }

        [Fact]
        public void AskQuestion_Test()
        {
            Game game = new Game(false);
        }

        [Fact]
        public void CurrentCategory_Test()
        {
            Game game = new Game(false);
            game.Add(new List<string>()
            {
                "Cat", "Dog", "Rat", "truc", "defef"
            });
            game.Roll(2);
            Assert.Equal(2, game.CurrentCategory());
        }

        [Fact]
        public void WasCorrectlyAnswered_InPenaltyBox_Test()
        {
            Game game = new Game(false);
        }

        [Fact]
        public void WasCorrectlyAnswered_NotInPenaltyBox_Test()
        {
            Game game = new Game(false);
        }

        [Fact]
        public void WrongAnswer_Test()
        {
            Game game = new Game(false);
        }

        [Fact]
        public void DidPlayerWin_Test()
        {
            Game game = new Game(false);
        } */

        [Fact]
        public void isPlayable_Test()
        {
            Game game = new Game(false);
            game.Add(new List<string>()
            {
                "Kevin", "Margot", "Rat", "truc", "defef"
            });
            Assert.True(game.IsPlayable());
        }
    }
}