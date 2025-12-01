using DefaultNamespace.Services;

namespace DefaultNamespace.Models
{
    public class Opponent : Player
    {
        private IStrategy _strategy;
        public Opponent(string name, IStrategy strategy) : base(name) => _strategy = strategy;

        public void SetStrategy(IStrategy strategy) => _strategy = strategy;

        public override void DecideTurn(DefaultNamespace.GameEngine engine) => _strategy.Execute(this, engine);
    }
}