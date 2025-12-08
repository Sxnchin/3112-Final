using DefaultNamespace.Enums;

namespace DefaultNamespace.Models
{
    public class Cashier : Employee
    {
        public Cashier(string name, int skill) : base(name, skill, EmployeeType.Cashier)
        {
        }

        public override TurnResult PerformTask(PlayerContext ctx)
        {
            var result = new TurnResult(Name);
            
            if (AssignedTask == "Sales")
            {
                result.SalesMultiplier = 1.0 + (SkillLevel * 0.05);
                result.Message = $"{Name} boosted sales!";
            }
            else
            {
                result.Message = $"{Name} is idle.";
            }
            
            result.Cost = 10;
            return result;
        }
    }
}