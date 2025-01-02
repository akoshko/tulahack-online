namespace Tulahack.UI.Storage;

public interface IState
{

}

public class SettingsState
{
}

public class AppState : IState
{
    public SettingsState SettingsState = new SettingsState()
    {
        // Difficulty = Difficulty.Medium,
        // DrawMode = DrawMode.DrawOne
    };

    public AppState()
    {
    }
};