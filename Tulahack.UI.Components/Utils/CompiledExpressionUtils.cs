using System.Linq.Expressions;

namespace Tulahack.UI.Components.Utils;

public static class CompiledExpressionUtils
{
    static DoubleParamMap<Type, string, Func<object, object>> _untypedGettersCache =
        new DoubleParamMap<Type, string, Func<object, object>>();

    public static Func<object, object> GetUntypedCSPropertyGetterByObjType
    (
        this Type objType,
        string propertyName
    )
    {
        Func<object, object> result;

        if (_untypedGettersCache.TryGetValue(objType, propertyName, out result))
        {
            return result;
        }

        ParameterExpression paramExpression = Expression.Parameter(typeof(object));
        UnaryExpression typedObjectExpression = Expression.Convert(paramExpression, objType);

        Expression propertyGetterExpression =
            Expression.Property(typedObjectExpression, propertyName);

        UnaryExpression valueCastExpression = Expression.Convert(propertyGetterExpression, typeof(object));

        result = Expression.Lambda<Func<object, object>>(valueCastExpression, paramExpression).Compile();

        _untypedGettersCache.AddKeyValue(objType, propertyName, result);

        return result;
    }
}