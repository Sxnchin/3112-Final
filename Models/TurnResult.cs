namespace DefaultNamespace.Models
{
    public class TurnResult
    {
        public string EmployeeName { get; }
        public double SalesMultiplier { get; set; } = 1.0;
        public double SpoilageMultiplier { get; set; } = 1.0;
        public double Cost { get; set; } = 0.0;
        public string Message { get; set; } = string.Empty;

        public TurnResult(string name) => EmployeeName = name;
    }
}