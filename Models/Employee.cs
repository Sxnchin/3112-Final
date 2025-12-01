using System;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Enums;

namespace DefaultNamespace.Models
{
    // Base class for employees
    public abstract class Employee : IAssignable, ITurnAction
    {
        // SOLID: SRP - handles only employee behavior and identity
        public string Name { get; }
        public int SkillLevel { get; } // 1..10
        public string AssignedTask { get; private set; } = "Idle";
        public EmployeeType Type { get; }

        protected Employee(string name, int skillLevel, EmployeeType type)
        {
            Name = name;
            SkillLevel = Math.Clamp(skillLevel, 1, 10);
            Type = type;
        }

        // IAssignable
        public void AssignTask(string task) => AssignedTask = task;

        // Subclasses implement task effects
        public abstract TurnResult PerformTask(PlayerContext ctx);

        // ITurnAction
        public void ExecuteTurn(PlayerContext ctx)
        {
            var result = PerformTask(ctx);
            ctx.RegisterEmployeeResult(result);
        }
    }
}
