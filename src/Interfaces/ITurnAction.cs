namespace DefaultNamespace.Interfaces
{
    // Turn-based action contract.
    // Use fully-qualified type in implementations to avoid circular references.
    public interface ITurnAction
    {
        void ExecuteTurn(DefaultNamespace.Models.PlayerContext ctx);
    }
}