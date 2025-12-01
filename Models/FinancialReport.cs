namespace DefaultNamespace.Models
{
    public struct FinancialReport
    {
        public double Revenue { get; set; }
        public double Expenses { get; set; }
        public double Profit => Revenue - Expenses;

        public FinancialReport(double revenue, double expenses)
        {
            Revenue = revenue;
            Expenses = expenses;
        }

        public double CalculateProfit() => Profit;
    }
}