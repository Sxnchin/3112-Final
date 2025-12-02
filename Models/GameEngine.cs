using System;
using System.Linq;
using DefaultNamespace.Enums;
using DefaultNamespace.Models;
using DefaultNamespace.Services;
using DefaultNamespace.Interfaces;

namespace DefaultNamespace
{
    public class GameEngine
    {
        // track per-turn financials for the final tally
        private readonly List<FinancialReport> _playerTurnReports = new List<FinancialReport>();
        private readonly List<FinancialReport> _opponentTurnReports = new List<FinancialReport>();
        private int _turn = 0;
        private const int MaxTurns = 7;

        public Player Player { get; }
        public Opponent Opponent { get; }
        public GameDifficulty Difficulty { get; }

        private readonly List<IGameObserver> _observers = new List<IGameObserver>();

        public GameEngine(GameDifficulty difficulty)
        {
            Difficulty = difficulty;

            // Initialize player
            Player = new Player("You");

            // Initialize opponent with difficulty-based strategy
            Opponent = new Opponent("Rival Co.", difficulty switch
            {
                GameDifficulty.Easy => new ConservativeStrategy(),
                GameDifficulty.Normal => new MixedStrategy(),
                GameDifficulty.Hard => new AggressiveStrategy(),
                GameDifficulty.Legend => new UltraAggressiveStrategy(),
                _ => new ConservativeStrategy()
            });

            // Skill boosts based on difficulty
            int skillBoost = difficulty switch
            {
                GameDifficulty.Easy => -1,
                GameDifficulty.Normal => 0,
                GameDifficulty.Hard => 1,
                GameDifficulty.Legend => 2,
                _ => 0
            };

            // Add opponent employees
            Opponent.Employees.Add(EmployeeFactory.Create(EmployeeType.Cashier, "Rival Cashier", 5 + skillBoost));
            Opponent.Employees.Add(EmployeeFactory.Create(EmployeeType.Manager, "Rival Manager", 6 + skillBoost));
            Opponent.Employees.Add(EmployeeFactory.Create(EmployeeType.Stocker, "Rival Stocker", 4 + skillBoost));

            // Add player employees
            Player.Employees.Add(EmployeeFactory.Create(EmployeeType.Cashier, "Yasuo", 6));
            Player.Employees.Add(EmployeeFactory.Create(EmployeeType.Stocker, "Mika", 5));
            Player.Employees.Add(EmployeeFactory.Create(EmployeeType.Manager, "Takeshi", 7));
        }

        public bool GameOver => _turn >= MaxTurns;

        // Observer pattern
        public void Subscribe(IGameObserver observer) => _observers.Add(observer);
        private void Notify(string message)
        {
            foreach (var o in _observers)
                o.Update(message);
        }

        // === Turn Logic ===
        public void NextTurn()
        {
            _turn++;
            Notify($"--- TURN {_turn} START ---");
            // Prepare market state and simulate market event
            var marketState = new MarketState();
            var marketEvent = MarketEventFactory.CreateRandomEvent(_turn);
            Notify($"Market Event: {marketEvent?.Name ?? "None"}");
            marketEvent?.AffectMarket(marketState);

            // Rival decisions
            Opponent.DecideTurn(this);
            Notify("Rival Co. made their decisions.");

            // Simulate results
            var (playerReport, opponentReport) = SimulateTurnAndGetReports(marketState);

            // Persist turn reports for final aggregation
            _playerTurnReports.Add(playerReport);
            _opponentTurnReports.Add(opponentReport);
            Notify($"--- Results ---");
            Notify($"Player: Rev ${playerReport.Revenue:F2} | Exp ${playerReport.Expenses:F2} | Profit ${playerReport.Profit:F2}");
            Notify($"Opponent: Rev ${opponentReport.Revenue:F2} | Exp ${opponentReport.Expenses:F2} | Profit ${opponentReport.Profit:F2}");

            if (GameOver)
            {
                Notify($"GAME OVER — {MaxTurns} days passed.");
                PrintFinalResults();
            }
        }

        public void QuickSimulateVerbose()
        {
            _turn++;
            Notify($"--- TURN {_turn} START ---");
            var marketState = new MarketState();
            var marketEvent = MarketEventFactory.CreateRandomEvent(_turn);
            Notify($"Market Event: {marketEvent?.Name ?? "None"}");
            marketEvent?.AffectMarket(marketState);

            Opponent.DecideTurn(this);
            Notify("Rival Co. made their decisions.");

            var (playerReport, opponentReport) = SimulateTurnAndGetReports(marketState);

            // Persist turn reports for final aggregation
            _playerTurnReports.Add(playerReport);
            _opponentTurnReports.Add(opponentReport);

            Notify($"Turn {_turn} Results:");
            Notify($"You - Revenue: ${playerReport.Revenue:F2}, Expenses: ${playerReport.Expenses:F2}, Profit: ${playerReport.Profit:F2}");
            Notify($"Rival Co. - Revenue: ${opponentReport.Revenue:F2}, Expenses: ${opponentReport.Expenses:F2}, Profit: ${opponentReport.Profit:F2}");

            if (GameOver)
            {
                Notify($"GAME OVER — {MaxTurns} days passed.");
                PrintFinalResults();
            }
        }

        // === Simulation ===
        public (FinancialReport player, FinancialReport opponent) SimulateTurnAndGetReports(MarketState market)
        {
            // More realistic revenue/expense calculation using prices, employees, and market
            var playerCtx = new PlayerContext(Player, market);
            var opponentCtx = new PlayerContext(Opponent, market);

            // Execute each employee's turn (register their effects)
            foreach (var e in Player.Employees)
                e.ExecuteTurn(playerCtx);
            foreach (var e in Opponent.Employees)
                e.ExecuteTurn(opponentCtx);

            // Notify employee outcomes so the UI can show messages
            foreach (var r in playerCtx.GetResults())
                Notify($"Player - {r.EmployeeName}: {r.Message} (Sales x{r.SalesMultiplier:F2}, Spoilage x{r.SpoilageMultiplier:F2}, Cost ${r.Cost:F2})");
            foreach (var r in opponentCtx.GetResults())
                Notify($"Opponent - {r.EmployeeName}: {r.Message} (Sales x{r.SalesMultiplier:F2}, Spoilage x{r.SpoilageMultiplier:F2}, Cost ${r.Cost:F2})");

            double playerRevenue = 0.0;
            double opponentRevenue = 0.0;

            // For each product, split market demand between the player and opponent
            foreach (var kv in Player.Prices)
            {
                var product = kv.Key;
                var playerPrice = kv.Value;
                var opponentPrice = Opponent.Prices.ContainsKey(product) ? Opponent.Prices[product] : playerPrice;

                // Base demand per product, adjusted by global market state
                double totalDemand = GameConfig.Instance.BaseDemand * market.GlobalDemandMultiplier;

                // Compute attractiveness weights: higher sales multiplier and lower price = higher weight
                double playerWeight = playerCtx.AggregateSalesMultiplier() / Math.Max(0.01, playerPrice);
                double opponentWeight = opponentCtx.AggregateSalesMultiplier() / Math.Max(0.01, opponentPrice);
                var sumWeight = playerWeight + opponentWeight;

                double playerShare = sumWeight <= 0 ? 0.5 : (playerWeight / sumWeight);
                double opponentShare = sumWeight <= 0 ? 0.5 : (opponentWeight / sumWeight);

                // Apply spoilage effects (reduce units sold)
                var playerSpoilage = playerCtx.AggregateSpoilageMultiplier();
                var opponentSpoilage = opponentCtx.AggregateSpoilageMultiplier();

                var playerUnits = totalDemand * playerShare * playerSpoilage;
                var opponentUnits = totalDemand * opponentShare * opponentSpoilage;

                playerRevenue += playerUnits * playerPrice;
                opponentRevenue += opponentUnits * opponentPrice;
            }

            // Expenses from employee costs
            double playerExpenses = playerCtx.TotalEmployeeCosts();
            double opponentExpenses = opponentCtx.TotalEmployeeCosts();

            var playerReport = new FinancialReport(playerRevenue, playerExpenses);
            var opponentReport = new FinancialReport(opponentRevenue, opponentExpenses);

            return (playerReport, opponentReport);
        }

        //  Final Results / Win Condition 
        private void PrintFinalResults()
        {
            Notify("");
            Notify("===== FINAL RESULTS =====");

            // compute the final results by aggregating all saved turn reports
            var playerReport = new FinancialReport(_playerTurnReports.Sum(r => r.Revenue), _playerTurnReports.Sum(r => r.Expenses));
            var opponentReport = new FinancialReport(_opponentTurnReports.Sum(r => r.Revenue), _opponentTurnReports.Sum(r => r.Expenses));

            Notify($"Your Final Profit: ${playerReport.Profit:F2}");
            Notify($"Opponent Final Profit: ${opponentReport.Profit:F2}");

            if (playerReport.Profit > opponentReport.Profit)
                Notify(" YOU WIN! Your business outperformed the rival!");
            else if (playerReport.Profit < opponentReport.Profit)
                Notify(" YOU LOSE! The rival dominated the market.");
            else
                Notify("It's a tie! You don't see that often!");
        }

        // Optional helper: print employees (used by UI)
        public void PrintAllEmployees()
        {
            Console.WriteLine("--- Your Employees ---");
            foreach (var e in Player.Employees)
                Console.WriteLine($"{e.Name} ({e.Type}) Skill:{e.SkillLevel} Task:{e.AssignedTask}");

            Console.WriteLine("--- Rival Employees ---");
            foreach (var e in Opponent.Employees)
                Console.WriteLine($"{e.Name} ({e.Type}) Skill:{e.SkillLevel} Task:{e.AssignedTask}");

            Console.WriteLine("--- Prices ---");
            foreach (var kv in Player.Prices)
                Console.WriteLine($"{kv.Key}: ${kv.Value:F2}");
        }
    }
}
