using System.Reflection;

namespace Tulahack.UI.Components.Utils;

public static class ReflectionUtils
{
    private static BindingFlags GetBindingFlags(bool includeNonPublic, bool isStatic)
    {
        BindingFlags bindingFlags =
            BindingFlags.Public;

        if (includeNonPublic)
        {
            bindingFlags |= BindingFlags.NonPublic;
        }

        if (isStatic)
        {
            bindingFlags |= BindingFlags.Static;
        }
        else
        {
            bindingFlags |= BindingFlags.Instance;
        }

        return bindingFlags;
    }

    public static PropertyInfo GetPropInfoFromType
    (
        this Type type,
        string propName,
        bool includeNonPublic = false,
        bool includeStatic = false)
    {
        BindingFlags bindingFlags = GetBindingFlags(includeNonPublic, includeStatic);

        PropertyInfo sourcePropInfo = type.GetProperty(propName, bindingFlags);

        return sourcePropInfo;
    }

    public static PropertyInfo GetPropInfo
    (
        this object obj,
        string propName,
        bool includeNonPublic = false)
    {
        PropertyInfo sourcePropInfo = obj.GetType().GetPropInfoFromType(propName, includeNonPublic);

        return sourcePropInfo;
    }

    public static object GetPropValue
    (
        this object obj,
        string propName,
        bool includeNonPublic = false)
    {
        PropertyInfo propInfo = obj.GetPropInfo(propName, includeNonPublic);

        return propInfo.GetValue(obj, null);
    }

    public static T GetPropValue<T>
    (
        this object obj,
        string propName,
        bool includeNonPublic = false) =>
        (T)obj.GetPropValue(propName, includeNonPublic);

    public static IEnumerable<Type> GetSelfSuperTypesAndInterfaces(this Type? type)
    {
        if (type == null)
        {
            yield break;
        }

        yield return type;

        if (type.BaseType != null)
        {
            foreach (Type superType in type.BaseType.GetSelfSuperTypesAndInterfaces())
            {
                yield return superType;
            }
        }

        foreach (Type interfaceType in type.GetInterfaces())
        {
            foreach (Type superInterface in interfaceType.GetSelfSuperTypesAndInterfaces())
            {
                yield return superInterface;
            }
        }
    }

    public static bool CheckConcreteTypeSatisfiesGenericParamConstraints(this Type concreteType,
        Type genericParamType)
    {
        var hasReferenceTypeConstraint =
            genericParamType.GenericParameterAttributes
                .HasFlag(GenericParameterAttributes.ReferenceTypeConstraint);

        var hasNewConstraint =
            genericParamType.GenericParameterAttributes
                .HasFlag(GenericParameterAttributes.DefaultConstructorConstraint);

        var isNonNullable =
            genericParamType.GenericParameterAttributes
                .HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint);

        if (hasReferenceTypeConstraint)
        {
            if (concreteType.IsValueType)
            {
                return false;
            }
        }
        else if (isNonNullable && !concreteType.IsValueType)
        {
            return true;
        }

        if (hasNewConstraint)
        {
            ConstructorInfo constrInfo =
                concreteType.GetConstructor([]);

            if (constrInfo == null)
            {
                return false;
            }
        }

        Type[] constraintTypes =
            genericParamType.GetGenericParameterConstraints();

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (constraintTypes == null)
        {
            return true;
        }

        foreach (Type constraintType in constraintTypes)
        {
            if (!concreteType.ResolveType(constraintType, constraintType.GetFromGenericType()))
            {
                return false;
            }
        }

        return true;
    }

    public static Type GetGenericMatchingSuperType(this Type typeToScan, Type typeToCompare)
    {
        Type? matchingSuperType =
            typeToScan.GetSelfSuperTypesAndInterfaces()
                .Distinct()
                .Where(arg => arg.IsGenericType)
                .FirstOrDefault(arg => arg.GetGenericTypeDefinition() ==
                                       typeToCompare.GetGenericTypeDefinition());

        return matchingSuperType;
    }

    // ReSharper disable InvalidXmlDocComment
    /// <summary>
    /// Checks if the argToResolveType can be used to resolve a
    /// genericArgType as a parameter in some
    /// method or class. E.g. for method static void DoSmth<TKey, TVal>(IDictionary<TKey, TVal> dict) where TVal : struct { }
    /// we can check if Dictionary<int, double> will fit to be passed as dict argument
    ///     (answer is yes, so it will return true)
    /// or we can check if Dictionary<int, object> will fit
    ///     (answer is no, because of the 'struct' constraint, so it will return false).
    /// If it can resolve all generic type parameters, it will also populate
    /// the ConcreteType property of members of the collection 'typesToConcretize' with
    /// the concrete types to replace the generic type parameters. In case of
    /// Dict<int, double>, the Concrete types for the corresponding collection members will be
    ///     typeof(int) and typeof(double).
    /// The typesToConcretize collection should already contain the
    /// entries corresponding to every generic parameter
    /// that the genericArgType depends on (for the example above, it will contain 2 entries -
    /// one for TKey and another for TVal).
    /// </summary>
    // ReSharper restore InvalidXmlDocComment
    public static bool ResolveType
    (
        this Type sourceArgType,
        Type? targetArgType,
        IEnumerable<IGenericParamInfo> genericTypeParamsToConcretize,
        bool resolveTypesFromSourceToTarget = true)
    {
        if (targetArgType == null)
        {
            return true;
        }

        if (resolveTypesFromSourceToTarget && targetArgType.IsGenericParameter ||
            !resolveTypesFromSourceToTarget && sourceArgType.IsGenericParameter)
        {
            if (!resolveTypesFromSourceToTarget)
            {
                (targetArgType, sourceArgType) = (sourceArgType, targetArgType);
            }

            IGenericParamInfo foundParamInfo =
                genericTypeParamsToConcretize.Single(t => t.GenericParameterType == targetArgType);

            if (!sourceArgType.CheckConcreteTypeSatisfiesGenericParamConstraints(
                    foundParamInfo.GenericParameterType))
            {
                return false;
            }

            foundParamInfo.PluggedInType = sourceArgType;

            return true;
        }
        if (targetArgType.IsGenericType)
        {
            Type matchingSuperType = sourceArgType.GetGenericMatchingSuperType(targetArgType);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (matchingSuperType == null)
            {
                return false;
            }

            Type[] sourceArgSubTypes = matchingSuperType.GetGenericArguments();
            Type[] targetArgSubTypes = targetArgType.GetGenericArguments();

            foreach ((Type sourceArgSubType, Type targetArgSubType) in sourceArgSubTypes.Zip(targetArgSubTypes,
                         (c, g) => (c, g)))
            {
                // ReSharper disable once PossibleMultipleEnumeration
                if (!sourceArgSubType.ResolveType(targetArgSubType, genericTypeParamsToConcretize,
                        resolveTypesFromSourceToTarget))
                {
                    return false;
                }
            }

            return true;
        }

        return targetArgType.IsAssignableFrom(sourceArgType);
    }

    public static MethodInfo GetMethod(this object obj, string methodName, bool includeNonPublic, bool isStatic,
        params Type[] argTypes)
    {
        BindingFlags bindingFlags = GetBindingFlags(includeNonPublic, isStatic);

        Type type = obj is Type t ? t : obj.GetType();

        var methodInfos = type.GetMember(methodName, bindingFlags).OfType<MethodInfo>().ToList();

        return methodInfos.FirstOrDefault(mInfo => mInfo.IsMethodCompatibleWithInputArgs(argTypes));
    }

    public static bool IsMethodCompatibleWithInputArgs(this MethodInfo method, params Type[] argRealTypes)
    {
        Type[] methodParamTypes = method.GetParameters().Select(x => x.ParameterType).ToArray();

        if (methodParamTypes.Length != argRealTypes.Length)
        {
            return false;
        }

        return methodParamTypes.Zip(argRealTypes).All(item => item.First.IsAssignableFrom(item.Second));
    }

    public static object CallMethodExtras
    (
        this object obj,
        string methodName,
        bool includeNonPublic,
        bool isStatic,
        params object[] args)
    {
        MethodInfo methodInfo =
            obj.GetMethod
            (
                methodName,
                includeNonPublic,
                isStatic,
                args.Select(arg => arg.GetType()).ToArray());

        return methodInfo.Invoke(obj, args);
    }

    public static bool IsConcrete(this Type? type) =>
        type != null && !type.GetAllGenericTypeParams().Any();

    public static IEnumerable<Type> GetAllGenericTypeParams(this Type? type)
    {
        if (type == null)
        {
            yield break;
        }

        if (type.IsGenericParameter)
        {
            yield return type;
        }

        if (type.IsGenericType)
        {
            foreach (Type genericArg in type.GetGenericArguments())
            {
                foreach (Type genericSubTypeParam in genericArg.GetAllGenericTypeParams())
                {
                    yield return genericSubTypeParam;
                }
            }
        }
        else if (type.IsArray)
        {
            Type elementType = type.GetElementType();

            if (elementType is null)
            {
                yield break;
            }

            foreach (Type genericSubTypeParam in elementType.GetAllGenericTypeParams())
            {
                yield return genericSubTypeParam;
            }
        }
    }
}
