using System;
using Tulahack.UI.Components.Controls.CodeBehind.Timeline;

namespace Tulahack.UI.ViewModels.Base;

public class TimelineItemViewModel
{
    public DateTime Time { get; set; }
    public string? TimeFormat { get; set; }
    public string? Description { get; set; }
    public string? Header { get; set; }
    public TimelineItemType ItemType { get; set; }
}
