using DefaultNamespace.Enums;

namespace DefaultNamespace.Models
{
    public class Stocker : Employee
    {
        public Stocker(string name, int skill) : base(name, skill, EmployeeType.Stocker)
        {
        }

        public override TurnResult PerformTask(PlayerContext ctx)
        {
            var result = new TurnResult(Name);
            
            if (AssignedTask == "Stock")
            {
                result.SpoilageMultiplier = 1.0 - (SkillLevel * 0.03);
                result.Message = $"{Name} reduced spoilage!";
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