namespace Tulahack.UI.Components.Utils;

public class DoubleParamMap<TOuterKeyType, TInnerKeyType, TInfoType>
    where TOuterKeyType : notnull
    where TInnerKeyType : notnull
    where TInfoType : notnull
{
    readonly Dictionary<TOuterKeyType, Dictionary<TInnerKeyType, TInfoType>> _dictionary = new();

    public void AddKeyValue
    (
        TOuterKeyType outerKey,
        TInnerKeyType innerKey,
        TInfoType infoData
    )
    {
        if (!_dictionary.TryGetValue(outerKey, out Dictionary<TInnerKeyType, TInfoType> innerDictionary))
        {
            innerDictionary = new Dictionary<TInnerKeyType, TInfoType>();

            _dictionary[outerKey] = innerDictionary;
        }

        innerDictionary[innerKey] = infoData;
    }

    public bool TryGetValue
    (
        TOuterKeyType outerKey,
        TInnerKeyType innerKey,
        out TInfoType? infoData
    )
    {
        infoData = default;

        if (
            (!_dictionary.TryGetValue(outerKey, out Dictionary<TInnerKeyType, TInfoType> innerDictionary)) ||
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            (innerDictionary == null)
        )
        {
            return false;
        }

        return
            innerDictionary.TryGetValue(innerKey, out infoData);
    }

    public bool ContainsKeys
    (
        TOuterKeyType outerKey,
        TInnerKeyType innerKey
    ) => TryGetValue(outerKey, innerKey, out TInfoType? _);

    public TInfoType GetValue
    (
        TOuterKeyType outerKey,
        TInnerKeyType innerKey
    )
    {
        if (!TryGetValue(outerKey, innerKey, out TInfoType result))
        {
            throw new Exception("There is no value in dictionary");
        }

        return result;
    }
}
