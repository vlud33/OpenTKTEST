using GameCore;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        using (Window game = new Window(new GameWindowSettings(), SetUpWindow()))
        {
            game.Run();
        }
    }

    private static NativeWindowSettings SetUpWindow()
    {
        NativeWindowSettings screenSettings = new NativeWindowSettings()
        {
            Size = new Vector2i(600, 600),
            Title = "TEST OPENTK"
        };

        return screenSettings;
    }
}