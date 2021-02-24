using System;
using System.Collections.Generic;
using System.Linq;
using Trivia;
using Xunit;

namespace Tests
{
    public class StatisticalyFairPlayers
    {
        [Fact]
        public void DefectTest()
        {
            Dictionary<string, Dictionary<string, int>> stats = new Dictionary<string, Dictionary<string, int>>(); 
            List<string> players = new List<string>()
            {
                "Alex", "Gab", "Nico"
            };            
            Random rand = new Random();
            Game game = new Game(true, rand);
            game.Add(players);
            int numberQuestion = 50000;
            for (int i = 0; i < numberQuestion * players.Count; i++)
            {
                game.Roll(rand.Next(5) + 1);
                if (!stats.ContainsKey(game.CurrentCategoryName))
                    stats.Add(game.CurrentCategoryName, new Dictionary<string, int>());
                
                if(!stats[game.CurrentCategoryName].ContainsKey(game.CurrentPlayer.ToString()))
                    stats[game.CurrentCategoryName].Add(game.CurrentPlayer.ToString(), 0);
                
                stats[game.CurrentCategoryName][game.CurrentPlayer.ToString()]++;
                game.SelectNextPlayer();
            }

            List<int> diff = new List<int>();
            foreach (KeyValuePair<string,Dictionary<string,int>> stat in stats)
            {
                Dictionary<string, int>.ValueCollection categoryCount = stat.Value.Values;
                int min = categoryCount.Min();
                int max = categoryCount.Max();
                diff.Add(max - min);
            }

            int sumDiff = diff.Sum();
            int diffAccount = sumDiff / diff.Count;
            float percent = (float)diffAccount / numberQuestion;
            Assert.True(percent < 0.01f, $"{percent} is not smaller than 0.05");
        }
        
    }
}