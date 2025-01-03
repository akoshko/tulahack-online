using System.Linq.Expressions;

namespace Tulahack.UI.Components.Utils;

public static class CompiledExpressionUtils
{
    static readonly DoubleParamMap<Type, string, Func<object, object>> UntypedGettersCache = new();

    public static Func<object, object> GetUntypedCsPropertyGetterByObjType
    (
        this Type objType,
        string propertyName
    )
    {
        if (UntypedGettersCache.TryGetValue(objType, propertyName, out Func<object, object> result))
        {
            return result;
        }

        ParameterExpression paramExpression = Expression.Parameter(typeof(object));
        UnaryExpression typedObjectExpression = Expression.Convert(paramExpression, objType);

        Expression propertyGetterExpression =
            Expression.Property(typedObjectExpression, propertyName);

        UnaryExpression valueCastExpression = Expression.Convert(propertyGetterExpression, typeof(object));

        result = Expression.Lambda<Func<object, object>>(valueCastExpression, paramExpression).Compile();

        UntypedGettersCache.AddKeyValue(objType, propertyName, result);

        return result;
    }
}
