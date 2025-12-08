using System;
using System.Linq;
using System.Collections.Generic;
using DefaultNamespace.Services;

namespace DefaultNamespace.Models
{
    // Responsible for simulating the business outcomes of a single turn
    public class TurnSimulator
    {
        public TurnSimulationResult SimulateTurn(Player player, Opponent opponent, MarketState market)
        {
            var result = new TurnSimulationResult();

            var playerCtx = new PlayerContext(player, market);
            var opponentCtx = new PlayerContext(opponent, market);

            foreach (var e in player.Employees)
                e.ExecuteTurn(playerCtx);
            foreach (var e in opponent.Employees)
                e.ExecuteTurn(opponentCtx);

            // Capture per-employee results for reporting
            result.PlayerResults = playerCtx.GetResults().ToList().AsReadOnly();
            result.OpponentResults = opponentCtx.GetResults().ToList().AsReadOnly();

            double playerRevenue = 0.0;
            double opponentRevenue = 0.0;

            foreach (var kv in player.Prices)
            {
                var product = kv.Key;
                var playerPrice = kv.Value;
                var opponentPrice = opponent.Prices.ContainsKey(product) ? opponent.Prices[product] : playerPrice;

                double totalDemand = GameConfig.Instance.BaseDemand * market.GlobalDemandMultiplier;

                double playerWeight = playerCtx.AggregateSalesMultiplier() / Math.Max(0.01, playerPrice);
                double opponentWeight = opponentCtx.AggregateSalesMultiplier() / Math.Max(0.01, opponentPrice);
                var sumWeight = playerWeight + opponentWeight;

                double playerShare = sumWeight <= 0 ? 0.5 : (playerWeight / sumWeight);
                double opponentShare = sumWeight <= 0 ? 0.5 : (opponentWeight / sumWeight);

                var playerSpoilage = playerCtx.AggregateSpoilageMultiplier();
                var opponentSpoilage = opponentCtx.AggregateSpoilageMultiplier();

                var playerUnits = totalDemand * playerShare * playerSpoilage;
                var opponentUnits = totalDemand * opponentShare * opponentSpoilage;

                playerRevenue += playerUnits * playerPrice;
                opponentRevenue += opponentUnits * opponentPrice;
            }

            double playerExpenses = playerCtx.TotalEmployeeCosts();
            double opponentExpenses = opponentCtx.TotalEmployeeCosts();

            result.PlayerReport = new FinancialReport(playerRevenue, playerExpenses);
            result.OpponentReport = new FinancialReport(opponentRevenue, opponentExpenses);

            return result;
        }
    }
}
