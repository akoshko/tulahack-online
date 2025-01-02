using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Media;

namespace Tulahack.UI.Components.Controls.CodeBehind.Timeline;

public class TimelineIconTemplateSelector : ResourceDictionary, IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is TimelineItemType t)
        {
            string s = t.ToString();

            if (ContainsKey(s))
            {
                object? o = this[s];

                if (o is SolidColorBrush c)
                {
                    var ellipse = new Ellipse() { Width = 12, Height = 12, Fill = c };
                    return ellipse;
                }
            }
        }

        return null;
    }

    public bool Match(object? data) =>
        data is TimelineItemType;
}
