namespace DefaultNamespace.Models
{
    public abstract class MarketEvent
    {
        public string Name { get; protected set; } = "GenericEvent";
        public double DemandChange { get; protected set; } = 0.0;
        public abstract void AffectMarket(MarketState state);
    }

    public class HolidayEvent : MarketEvent
    {
        public HolidayEvent()
        {
            Name = "Holiday";
            DemandChange = 0.35;
        }
        public override void AffectMarket(MarketState state) => state.GlobalDemandMultiplier += DemandChange;
    }

    public class SlowDayEvent : MarketEvent
    {
        public SlowDayEvent()
        {
            Name = "Slow Day";
            DemandChange = -0.25;
        }
        public override void AffectMarket(MarketState state) => state.GlobalDemandMultiplier += DemandChange;
    }

    public class LayoffsEvent : MarketEvent
    {
        public LayoffsEvent()
        {
            Name = "Corporate Layoffs";
            DemandChange = -0.45;
        }
        public override void AffectMarket(MarketState state) => state.GlobalDemandMultiplier += DemandChange;
    }
}