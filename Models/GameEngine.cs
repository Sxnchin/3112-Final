using System;
using System.Linq;
using DefaultNamespace.Enums;
using DefaultNamespace.Models;
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
        private readonly TurnExecutor _turnExecutor;
        private readonly ResultsProcessor _resultsProcessor;

        public GameEngine(GameDifficulty difficulty)
        {
            Difficulty = difficulty;

            // Initialize game
            (Player, Opponent) = GameInitializer.InitializeGame(difficulty);

            // Initialize collaborators
            _turnExecutor = new TurnExecutor(_observers);
            _resultsProcessor = new ResultsProcessor(_observers);
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

            var simResult = _turnExecutor.ExecuteTurn(_turn, Player, Opponent);

            _playerTurnReports.Add(simResult.PlayerReport);
            _opponentTurnReports.Add(simResult.OpponentReport);

            if (GameOver)
            {
                Notify($"GAME OVER — {MaxTurns} days passed.");
                _resultsProcessor.ProcessFinalResults(_playerTurnReports, _opponentTurnReports);
            }
        }

        public void QuickSimulateVerbose()
        {
            _turn++;

            var simResult = _turnExecutor.ExecuteTurn(_turn, Player, Opponent);

            _playerTurnReports.Add(simResult.PlayerReport);
            _opponentTurnReports.Add(simResult.OpponentReport);

            Notify($"Turn {_turn} Results:");
            Notify($"You - Revenue: ${simResult.PlayerReport.Revenue:F2}, Expenses: ${simResult.PlayerReport.Expenses:F2}, Profit: ${simResult.PlayerReport.Profit:F2}");
            Notify($"Rival Co. - Revenue: ${simResult.OpponentReport.Revenue:F2}, Expenses: ${simResult.OpponentReport.Expenses:F2}, Profit: ${simResult.OpponentReport.Profit:F2}");

            if (GameOver)
            {
                Notify($"GAME OVER — {MaxTurns} days passed.");
                _resultsProcessor.ProcessFinalResults(_playerTurnReports, _opponentTurnReports);
            }
        }


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
