using DefaultNamespace.Models;
using DefaultNamespace.Enums;

namespace DefaultNamespace
{
    public static class EmployeeFactory
    {
        public static Employee Create(EmployeeType type, string name, int skill)
        {
            return type switch
            {
                EmployeeType.Cashier => new Cashier(name, skill),
                EmployeeType.Stocker => new Stocker(name, skill),
                EmployeeType.Manager => new Manager(name, skill),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }
    }
}