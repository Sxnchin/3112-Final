using DefaultNamespace.Enums;
using DefaultNamespace.UI;

namespace DefaultNamespace
{
    class Program
    {
        static void Main()
        {
            var ui = new ConsoleUI(null);

            var difficulty = ui.SelectDifficultyExternal();

            var engine = new GameEngine(difficulty);

            ui.SetEngine(engine);
            engine.Subscribe(ui);

            ui.Run();
        }
    }
}