using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace.Models
{
    public class PlayerContext
    {
        public Player Player { get; }
        public MarketState Market { get; }

        private readonly List<TurnResult> _results = new List<TurnResult>();

        public PlayerContext(Player player, MarketState market)
        {
            Player = player;
            Market = market;
        }

        public void RegisterEmployeeResult(TurnResult r) => _results.Add(r);

        public double AggregateSalesMultiplier()
        {
            double m = 1.0;
            foreach (var r in _results) m *= r.SalesMultiplier;
            return m;
        }

        public double AggregateSpoilageMultiplier()
        {
            double m = 1.0;
            foreach (var r in _results) m *= r.SpoilageMultiplier;
            return m;
        }

        public double TotalEmployeeCosts() => _results.Sum(r => r.Cost);
    }
}