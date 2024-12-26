namespace Tulahack.UI.Storage;

public interface IState
{
    
}

public class SettingsState
{
    // public Difficulty Difficulty { get; set; }
    // public DrawMode DrawMode { get; set; }
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