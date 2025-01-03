namespace Tulahack.UI.Components.Utils;

public interface IGenericParamInfo
{
    Type GenericParameterType { get; }

    string GenericParamTypeName => GenericParameterType.Name;

    // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
    bool IsResolved => PluggedInType != null;

    Type PluggedInType { get; set; }
}

public sealed class GenericParamInfoBase : IGenericParamInfo
{
    public Type GenericParameterType { get; }

    public string GenericParamTypeName => GenericParameterType.Name;

    // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
    public bool IsResolved => PluggedInType != null /* && PluggedInType.IsConcrete() */;

    public Type PluggedInType { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is IGenericParamInfo paramObserver)
        {
            return GenericParameterType == paramObserver.GenericParameterType;
        }

        return false;
    }


    /// <summary>
    /// for test purpose only
    /// </summary>
    public void SetPluggedInTypeToInt() =>
        PluggedInType = typeof(int);

    public override int GetHashCode() =>
        GenericParameterType.GetHashCode();

    public GenericParamInfoBase
    (
        Type genericParamType,
        Type pluggedInType)
    {
        GenericParameterType = genericParamType;
        PluggedInType = pluggedInType;
    }

    public GenericParamInfoBase
    (
        Type genericParamType)
    {
        GenericParameterType = genericParamType;
        PluggedInType = typeof(int);
    }

    public void ResetConcreteType() =>
        PluggedInType = typeof(int);
}

public static class GenericParamsExtensions
{
    public static bool AreAllResolved(this IEnumerable<IGenericParamInfo>? genericParamInfos) =>
        genericParamInfos?.All(paramInfo => paramInfo.IsResolved) ?? true;

    public static Type[] GetConcreteTypes(this IEnumerable<IGenericParamInfo> genericParamInfos) =>
        genericParamInfos.Select(paramInfo => paramInfo.PluggedInType).ToArray();

    public static IGenericParamInfo GetGenericParamInfo(this IEnumerable<IGenericParamInfo> genericParams,
        Type genericType) =>
        genericParams
            .Single(genParamInfo => genParamInfo.GenericParameterType == genericType);

    public static IGenericParamInfo[] GetFromGenericType
    (
        this Type genericType,
        IEnumerable<IGenericParamInfo>? genericParamInfosToChooseFrom = null,
        Func<Type, IGenericParamInfo>? factory = null)
    {
        IGenericParamInfo[] result =
            genericType.GetAllGenericTypeParams()
                .Distinct()
                .Select
                (
                    genArgType => genericParamInfosToChooseFrom?.GetGenericParamInfo(genArgType) ??
                                  factory?.Invoke(genArgType) ??
                                  new GenericParamInfoBase(genArgType)
                )
                .ToArray();

        return result;
    }
}
