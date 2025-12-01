using DefaultNamespace.Enums;

namespace DefaultNamespace.Models
{
    public class Manager : Employee
    {
        public Manager(string name, int skill) : base(name, skill, EmployeeType.Manager)
        {
        }

        public override TurnResult PerformTask(PlayerContext ctx)
        {
            var result = new TurnResult(Name);
            
            if (AssignedTask == "Lead")
            {
                result.SalesMultiplier = 1.0 + (SkillLevel * 0.07);
                result.Message = $"{Name} led the team effectively!";
            }
            else
            {
                result.Message = $"{Name} is idle.";
            }
            
            result.Cost = 15;
            return result;
        }
    }
}