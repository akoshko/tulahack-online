using System.Text.Json.Serialization;
using Tulahack.Dtos;
using Tulahack.UI.Storage;

namespace Tulahack.UI.Utils;
// https://learn.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/configure-trimmer?view=aspnetcore-8.0
// https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/prepare-libraries-for-trimming
// https://devblogs.microsoft.com/dotnet/system-text-json-in-dotnet-8/#disabling-reflection-defaults
// https://www.twilio.com/en-us/blog/use-jsonserializeroptions-with-source-generated-json-deserialization

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    IncludeFields = true,
    WriteIndented = false)]
[JsonSerializable(typeof(DashboardDto))]
[JsonSerializable(typeof(ContestTimelineDto))]
[JsonSerializable(typeof(TimelineItemDto))]
[JsonSerializable(typeof(ContestantDto))]
[JsonSerializable(typeof(CompanyDto))]
[JsonSerializable(typeof(SkillDto))]
[JsonSerializable(typeof(StorageFileDto))]
[JsonSerializable(typeof(TeamDto))]
[JsonSerializable(typeof(UserPreferencesDto))]
[JsonSerializable(typeof(ContestApplicationDto))]
[JsonSerializable(typeof(IState))]
public partial class TulahackJsonContext : JsonSerializerContext {}