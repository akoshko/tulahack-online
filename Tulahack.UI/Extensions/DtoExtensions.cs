using System;
using Tulahack.Dtos;
using Tulahack.UI.Components.Controls.CodeBehind.Timeline;

namespace Tulahack.UI.Extensions;

public static class DtoExtensions
{
    public static TimelineItemType ConvertToControlType(this TimelineItemTypeDto dto)
    {
        switch (dto)
        {
            case TimelineItemTypeDto.Checkpoint:
                return TimelineItemType.Error;
            case TimelineItemTypeDto.Deadline:
                return TimelineItemType.Warning;
            case TimelineItemTypeDto.Event:
                return TimelineItemType.Success;
            case TimelineItemTypeDto.Meetup:
                return TimelineItemType.Ongoing;
            default:
                return TimelineItemType.Default;
        }
    }
}