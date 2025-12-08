using System;
using DefaultNamespace.Models;

namespace DefaultNamespace.Services
{
    public static class MarketEventFactory
    {
        public static MarketEvent CreateRandomEvent(int seed)
        {
            var r = new Random(seed);
            var pick = r.Next(0, 3);
            return pick switch
            {
                0 => new HolidayEvent(),
                1 => new SlowDayEvent(),
                2 => new LayoffsEvent(),
                _ => new SlowDayEvent()
            };
        }
    }
}