namespace DefaultNamespace.Interfaces
{
    // Decision maker for players / AI
    public interface IDecisionMaker
    {
        void DecideTurn(DefaultNamespace.GameEngine engine);
    }
}