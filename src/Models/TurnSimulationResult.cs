using System.Collections.Generic;

namespace DefaultNamespace.Models
{
    public class TurnSimulationResult
    {
        public FinancialReport PlayerReport { get; set; }
        public FinancialReport OpponentReport { get; set; }
        public IReadOnlyList<TurnResult> PlayerResults { get; set; } = new List<TurnResult>();
        public IReadOnlyList<TurnResult> OpponentResults { get; set; } = new List<TurnResult>();
    }
}
