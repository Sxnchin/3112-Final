using DefaultNamespace.Enums;
using DefaultNamespace.Models;
using DefaultNamespace.Interfaces;

namespace DefaultNamespace.UI
{
    public class ConsoleUI : IGameObserver
    {
        private GameEngine _engine;

        public ConsoleUI(GameEngine engine)
        {
            _engine = engine;
        }

        public void SetEngine(GameEngine engine)
        {
            _engine = engine;
        }

        public void Update(string message)
        {
            Console.WriteLine(message);
        }

        public void Run()
        {
            Console.Clear();
            Welcome();

            while (true)
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1) View Employees");
                Console.WriteLine("2) Assign Employee Task");
                Console.WriteLine("3) Adjust Prices");
                Console.WriteLine("4) Resolve Turn");
                Console.WriteLine("5) Quick Simulate Turn (shows detailed outputs)");
                Console.WriteLine("Q) Quit");
                Console.Write("Choice: ");

                var input = Console.ReadLine();
                if (input?.ToUpper() == "Q")
                {
                    Console.WriteLine("Thanks for playing. Press any key to exit.");
                    Console.ReadKey();
                    return;
                }

                switch (input)
                {
                    case "1":
                        _engine.PrintAllEmployees();
                        break;
                    case "2":
                        AssignTaskMenu();
                        break;
                    case "3":
                        AdjustPriceMenu();
                        break;
                    case "4":
                        _engine.NextTurn();
                        break;
                    case "5":
                        _engine.QuickSimulateVerbose();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private void AssignTaskMenu()
        {
            Console.WriteLine("Select employee by number:");
            var employees = _engine.Player.Employees;

            for (int i = 0; i < employees.Count; i++)
            {
                Console.WriteLine($"{i}) {employees[i].Name} ({employees[i].Type}) - Task: {employees[i].AssignedTask}");
            }

            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 0 || idx >= employees.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            Console.Write("Enter task (Sales / Stock / Lead / Idle): ");
            var task = Console.ReadLine();

            employees[idx].AssignTask(task);
            Console.WriteLine($"Assigned {employees[idx].Name} to {task}");
        }

        private void AdjustPriceMenu()
        {
            Console.WriteLine("Products:");
            var prices = _engine.Player.Prices.ToList();

            for (int i = 0; i < prices.Count; i++)
                Console.WriteLine($"{i}) {prices[i].Key} - ${prices[i].Value:F2}");

            Console.Write("Select product by number: ");
            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 0 || idx >= prices.Count)
            {
                Console.WriteLine("Invalid index.");
                return;
            }

            Console.Write($"Enter new price for {prices[idx].Key}: ");
            if (!double.TryParse(Console.ReadLine(), out double newPrice))
            {
                Console.WriteLine("Invalid price.");
                return;
            }

            _engine.Player.Prices[prices[idx].Key] = newPrice;
            Console.WriteLine($"{prices[idx].Key} price set to ${newPrice:F2}");
        }

        public void Welcome()
        {
            Console.WriteLine("Welcome to Dragon Manager - Business Minigame");
            Console.WriteLine("Manage employees, set prices, respond to market events, and beat the rival.");
            Console.WriteLine();
        }

        // Difficulty selection BEFORE engine exists
        public GameDifficulty SelectDifficultyExternal()
        {
            Console.WriteLine("Select Difficulty:");
            Console.WriteLine("1) Easy");
            Console.WriteLine("2) Normal");
            Console.WriteLine("3) Hard");
            Console.WriteLine("4) Legend");
            Console.Write("Choice: ");

            return Console.ReadLine() switch
            {
                "1" => GameDifficulty.Easy,
                "2" => GameDifficulty.Normal,
                "3" => GameDifficulty.Hard,
                "4" => GameDifficulty.Legend,
                _ => GameDifficulty.Normal
            };
        }
    }
}
