using DefaultNamespace.Models;

namespace DefaultNamespace.Services
{
    // Responsible for creating and applying market events for a turn
    public class MarketService
    {
        public (MarketState market, MarketEvent? marketEvent) CreateMarketForTurn(int turn)
        {
            var market = new MarketState();
            var ev = MarketEventFactory.CreateRandomEvent(turn);
            ev?.AffectMarket(market);
            return (market, ev);
        }
    }
}
