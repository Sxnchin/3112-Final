using DefaultNamespace.Enums;
using DefaultNamespace.Services;

namespace DefaultNamespace.Models
{
    public class GameInitializer
    {
        public static (Player player, Opponent opponent) InitializeGame(GameDifficulty difficulty)
        {
            var player = new Player("You");
            var opponent = CreateOpponent(difficulty);

            AddPlayerEmployees(player);
            AddOpponentEmployees(opponent, difficulty);

            return (player, opponent);
        }

        private static Opponent CreateOpponent(GameDifficulty difficulty)
        {
            var strategy = difficulty switch
            {
                GameDifficulty.Easy => new ConservativeStrategy(),
                GameDifficulty.Normal => new MixedStrategy(),
                GameDifficulty.Hard => new AggressiveStrategy(),
                GameDifficulty.Legend => new UltraAggressiveStrategy(),
                _ => new ConservativeStrategy()
            };

            return new Opponent("Rival Co.", strategy);
        }

        private static void AddPlayerEmployees(Player player)
        {
            player.Employees.Add(EmployeeFactory.Create(EmployeeType.Cashier, "Yasuo", 6));
            player.Employees.Add(EmployeeFactory.Create(EmployeeType.Stocker, "Mika", 5));
            player.Employees.Add(EmployeeFactory.Create(EmployeeType.Manager, "Takeshi", 7));
        }

        private static void AddOpponentEmployees(Opponent opponent, GameDifficulty difficulty)
        {
            int skillBoost = difficulty switch
            {
                GameDifficulty.Easy => -1,
                GameDifficulty.Normal => 0,
                GameDifficulty.Hard => 1,
                GameDifficulty.Legend => 2,
                _ => 0
            };

            opponent.Employees.Add(EmployeeFactory.Create(EmployeeType.Cashier, "Rival Cashier", 5 + skillBoost));
            opponent.Employees.Add(EmployeeFactory.Create(EmployeeType.Manager, "Rival Manager", 6 + skillBoost));
            opponent.Employees.Add(EmployeeFactory.Create(EmployeeType.Stocker, "Rival Stocker", 4 + skillBoost));
        }
    }
}
