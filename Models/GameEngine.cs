using DefaultNamespace.Enums;
using DefaultNamespace.Models;
using DefaultNamespace.Services;
using DefaultNamespace.Interfaces;

namespace DefaultNamespace
{
    public class GameEngine
    {
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

            // Simulate market event
            var marketEvent = MarketEventFactory.CreateRandomEvent(_turn);
            Notify($"Market Event: {marketEvent?.Name ?? "None"}");

            // Rival decisions
            Opponent.DecideTurn(this);
            Notify("Rival Co. made their decisions.");

            // Simulate results
            var (playerReport, opponentReport) = SimulateTurnAndGetReports();
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

            var marketEvent = MarketEventFactory.CreateRandomEvent(_turn);
            Notify($"Market Event: {marketEvent?.Name ?? "None"}");

            Opponent.DecideTurn(this);
            Notify("Rival Co. made their decisions.");

            var (playerReport, opponentReport) = SimulateTurnAndGetReports();

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
        public (FinancialReport player, FinancialReport opponent) SimulateTurnAndGetReports()
        {
            // Simplified revenue/expense calculation
            double playerRevenue = Player.Employees.Sum(e => e.SkillLevel * 50);
            double opponentRevenue = Opponent.Employees.Sum(e => e.SkillLevel * 50);

            double playerExpenses = Player.Employees.Sum(e => 10);
            double opponentExpenses = Opponent.Employees.Sum(e => 10);

            var playerReport = new FinancialReport(playerRevenue, playerExpenses);
            var opponentReport = new FinancialReport(opponentRevenue, opponentExpenses);

            return (playerReport, opponentReport);
        }

        // === Final Results / Win Condition ===
        private void PrintFinalResults()
        {
            Notify("");
            Notify("===== FINAL RESULTS =====");

            var (playerReport, opponentReport) = SimulateTurnAndGetReports();

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
