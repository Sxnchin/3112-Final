using System;
using System.Collections.Generic;
using DefaultNamespace.Services;
using DefaultNamespace.Interfaces;

namespace DefaultNamespace.Models
{
    public class TurnExecutor
    {
        private readonly MarketService _marketService;
        private readonly TurnSimulator _turnSimulator;
        private readonly List<IGameObserver> _observers;

        public TurnExecutor(List<IGameObserver> observers)
        {
            _marketService = new MarketService();
            _turnSimulator = new TurnSimulator();
            _observers = observers;
        }

        public TurnSimulationResult ExecuteTurn(int turnNumber, Player player, Opponent opponent)
        {
            Notify($"--- TURN {turnNumber} START ---");

            // Create market state and event
            var (marketState, marketEvent) = _marketService.CreateMarketForTurn(turnNumber);
            Notify($"Market Event: {marketEvent?.Name ?? "None"}");

            // Get rival decisions
            opponent.DecideTurn(null);
            Notify("Rival Co. made their decisions.");

            // Simulate turn
            var simResult = _turnSimulator.SimulateTurn(player, opponent, marketState);

            // Notify results
            NotifyTurnResults(simResult);

            return simResult;
        }

        private void NotifyTurnResults(TurnSimulationResult simResult)
        {
            Notify($"--- Results ---");
            Notify($"Player: Rev ${simResult.PlayerReport.Revenue:F2} | Exp ${simResult.PlayerReport.Expenses:F2} | Profit ${simResult.PlayerReport.Profit:F2}");
            Notify($"Opponent: Rev ${simResult.OpponentReport.Revenue:F2} | Exp ${simResult.OpponentReport.Expenses:F2} | Profit ${simResult.OpponentReport.Profit:F2}");

            foreach (var r in simResult.PlayerResults)
                Notify($"Player - {r.EmployeeName}: {r.Message} (Sales x{r.SalesMultiplier:F2}, Spoilage x{r.SpoilageMultiplier:F2}, Cost ${r.Cost:F2})");
            foreach (var r in simResult.OpponentResults)
                Notify($"Opponent - {r.EmployeeName}: {r.Message} (Sales x{r.SalesMultiplier:F2}, Spoilage x{r.SpoilageMultiplier:F2}, Cost ${r.Cost:F2})");
        }

        private void Notify(string message)
        {
            foreach (var o in _observers)
                o.Update(message);
        }
    }
}
