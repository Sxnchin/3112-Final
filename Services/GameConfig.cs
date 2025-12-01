namespace DefaultNamespace.Services
{
    public sealed class GameConfig
    {
        private static readonly GameConfig _instance = new GameConfig();
        public static GameConfig Instance => _instance;

        private GameConfig() { BaseDemand = 100; }

        public int BaseDemand { get; set; }
    }
}