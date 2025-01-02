namespace Tulahack.UI.Storage;

public interface IState
{

}

public class SettingsState
{
}

public class AppState : IState
{
    public SettingsState SettingsState = new()
    {
        // Difficulty = Difficulty.Medium,
        // DrawMode = DrawMode.DrawOne
    };
};