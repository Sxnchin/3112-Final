using DefaultNamespace.Enums;
using DefaultNamespace.Models;

namespace DefaultNamespace.Services
{
    // Responsible for creating players and seeding employees
    public class GameSetup
    {
        public Player CreatePlayer(string name)
        {
            var player = new Player(name);
            player.Employees.Add(EmployeeFactory.Create(EmployeeType.Cashier, "Yasuo", 6));
            player.Employees.Add(EmployeeFactory.Create(EmployeeType.Stocker, "Mika", 5));
            player.Employees.Add(EmployeeFactory.Create(EmployeeType.Manager, "Takeshi", 7));
            return player;
        }

        public Opponent CreateOpponent(string name, GameDifficulty difficulty)
        {
            int skillBoost = difficulty switch
            {
                GameDifficulty.Easy => -1,
                GameDifficulty.Normal => 0,
                GameDifficulty.Hard => 1,
                GameDifficulty.Legend => 2,
                _ => 0
            };

            var strategy = difficulty switch
            {
                GameDifficulty.Easy => new ConservativeStrategy(),
                GameDifficulty.Normal => new MixedStrategy(),
                GameDifficulty.Hard => new AggressiveStrategy(),
                GameDifficulty.Legend => new UltraAggressiveStrategy(),
                _ => new ConservativeStrategy()
            };

            var opponent = new Opponent(name, strategy);
            opponent.Employees.Add(EmployeeFactory.Create(EmployeeType.Cashier, "Rival Cashier", 5 + skillBoost));
            opponent.Employees.Add(EmployeeFactory.Create(EmployeeType.Manager, "Rival Manager", 6 + skillBoost));
            opponent.Employees.Add(EmployeeFactory.Create(EmployeeType.Stocker, "Rival Stocker", 4 + skillBoost));
            return opponent;
        }
    }
}
