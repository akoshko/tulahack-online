using System.ComponentModel;

namespace Tulahack.UI.Models;

/// <summary>
/// Contest case complexity, describes how hard it is to complete the task
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.2.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
public enum CaseComplexitySelector
{

    [System.Runtime.Serialization.EnumMember(Value = @"Unknown")]
    [Description("Не указано")]
    Unknown = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"Beginner")]
    [Description("Для начинающих")]
    Beginner = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"Easy")]
    [Description("Просто")]
    Easy = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"Normal")]
    [Description("Нормально")]
    Normal = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"Hard")]
    [Description("Сложно")]
    Hard = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"Insane")]
    [Description("Хардкор")]
    Insane = 5,
}

/// <summary>
/// In person or online, очно или онлайн
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.2.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
public enum FormOfParticipationSelector
{

    [System.Runtime.Serialization.EnumMember(Value = @"Unknown")]
    [Description("Не указано")]
    Unknown = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"InPerson")]
    [Description("На площадке")]
    InPerson = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"Online")]
    [Description("Удаленно")]
    Online = 2,
}

/// <summary>
/// In person or online, очно или онлайн
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.2.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
public enum UserPreferredThemeSelector
{

    [System.Runtime.Serialization.EnumMember(Value = @"Default")]
    [Description("По умолчанию")]
    Default = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"Light")]
    [Description("Светлая")]
    Light = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"Dark")]
    [Description("Темная")]
    Dark = 2,
}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.2.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
public enum ContestRoleSelector
{

    [System.Runtime.Serialization.EnumMember(Value = @"Visitor")]
    [Description("Зритель")]
    Visitor = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"Contestant")]
    [Description("Участник")]
    Contestant = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"Expert")]
    [Description("Эксперт")]
    Expert = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"Moderator")]
    [Description("Модератор")]
    Moderator = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"Superuser")]
    [Description("Суперпользователь")]
    Superuser = 4,
}