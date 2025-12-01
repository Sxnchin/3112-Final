using System.Collections.Generic;
using DefaultNamespace.Interfaces;

namespace DefaultNamespace.Models
{
    public class Player : IDecisionMaker
    {
        public string Name { get; }
        public List<Employee> Employees { get; } = new List<Employee>();
        public Dictionary<string, double> Prices { get; } = new Dictionary<string, double>();

        public Player(string name)
        {
            Name = name;
            Prices["Noodles"] = 5.00;
            Prices["Drink"] = 2.50;
            Prices["Snack"] = 3.25;
        }

        // Human player: UI will guide decisions
        public virtual void DecideTurn(DefaultNamespace.GameEngine engine) { }

        public bool AssignEmployee(Employee e, string task)
        {
            if (!Employees.Contains(e)) return false;
            e.AssignTask(task);
            return true;
        }

        public void AdjustPrice(string product, double newPrice)
        {
            if (Prices.ContainsKey(product))
                Prices[product] = System.Math.Max(0.5, newPrice);
        }
    }
}