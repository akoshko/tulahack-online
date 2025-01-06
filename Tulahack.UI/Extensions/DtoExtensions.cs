using Tulahack.Dtos;
using Tulahack.UI.Components.Controls.CodeBehind.Timeline;

namespace Tulahack.UI.Extensions;

public static class DtoExtensions
{
    public static TimelineItemType ConvertToControlType(this TimelineItemTypeDto dto) =>
        dto switch
        {
            TimelineItemTypeDto.Checkpoint => TimelineItemType.Error,
            TimelineItemTypeDto.Deadline => TimelineItemType.Warning,
            TimelineItemTypeDto.Event => TimelineItemType.Success,
            TimelineItemTypeDto.Meetup => TimelineItemType.Ongoing,
            _ => TimelineItemType.Default
        };
}