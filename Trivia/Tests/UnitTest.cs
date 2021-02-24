using System;
using Xunit;
using Trivia;
using System.Collections.Generic;

namespace Tests
{
    public class UnitTest
    {
        /*
        [Fact]
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
        public class IsPlayableTests
        {
            [Fact]
            public void isPlayable_enough()
            {
                // INIT VAR
                Game game = new Game(false, new Random());
                game.Add(new List<string>()
                {
                    "Kevin", "Margot", "Rat", "truc", "defef"
                });
                //ASSERT
                Assert.True(game.IsPlayable());
            }
            [Fact]
            public void isPlayable_tooMuch()
            {
                // INIT VAR
                Game game = new Game(false, new Random());
                game.Add(new List<string>()
                {
                    "Kevin", "Margot", "Stéphane", "Tintin", "Nicolas", "Jean", "Michel", "David", "Olivier"
                });
                //ASSERT
                Assert.False(game.IsPlayable());
            }
            [Fact]
            public void isPlayable_notEnough()
            {
                // INIT VAR
                Game game = new Game(false, new Random());
                game.Add(new List<string>()
                {
                    "Kevin"
                });
                //ASSERT
                Assert.False(game.IsPlayable());
            }
        }
    }
    
}