using System;
using System.Linq;
using DefaultNamespace.Models;

namespace DefaultNamespace.Services
{
    public interface IStrategy
    {
        void Execute(Opponent opponent, DefaultNamespace.GameEngine engine);
    }

    public class ConservativeStrategy : IStrategy
    {
        public void Execute(Opponent opponent, DefaultNamespace.GameEngine engine)
        {
            foreach (var p in opponent.Prices.Keys.ToList())
                opponent.Prices[p] = Math.Max(0.9 * opponent.Prices[p], 0.5);

            foreach (var e in opponent.Employees)
                e.AssignTask("Stock");
        }
    }

    public class AggressiveStrategy : IStrategy
    {
        public void Execute(Opponent opponent, DefaultNamespace.GameEngine engine)
        {
            var top = opponent.Prices.OrderByDescending(x => x.Value).First().Key;
            opponent.Prices[top] *= 1.10;

            foreach (var e in opponent.Employees)
                e.AssignTask(e is Manager ? "Lead" : "Sales");
        }
    }
    
    public class MixedStrategy : IStrategy
    {
        private readonly Random _rng = new Random();

        public void Execute(Opponent opponent, DefaultNamespace.GameEngine engine)
        {
            if (_rng.NextDouble() < 0.5)
                new AggressiveStrategy().Execute(opponent, engine);
            else
                new ConservativeStrategy().Execute(opponent, engine);
        }
    }

    public class UltraAggressiveStrategy : IStrategy
    {
        public void Execute(Opponent opponent, DefaultNamespace.GameEngine engine)
        {
            foreach (var p in opponent.Prices.Keys.ToList())
                opponent.Prices[p] *= 1.15;

            foreach (var e in opponent.Employees)
                e.AssignTask(e is Manager ? "Lead" : "Sales");
        }
    }
}