namespace DefaultNamespace.Models
{
    public class MarketState
    {
        // Default to 1.0 (no change). Market events will modify this additively.
        public double GlobalDemandMultiplier { get; set; } = 1.0;
    }
}