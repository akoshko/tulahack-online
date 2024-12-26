namespace Tulahack.UI.Components.Contracts;

public interface IDialogContext
{
    public void Close();
    public event EventHandler<object?>? RequestClose;
}