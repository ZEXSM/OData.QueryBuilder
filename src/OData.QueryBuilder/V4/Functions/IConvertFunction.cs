using System;

namespace OData.QueryBuilder.V4.Functions
{
    public interface IConvertFunction
    {
        T ConvertEnumToString<T>(T type) where T : Enum;

        DateTime ConvertDateTimeToString(DateTime dateTime, string format);

        DateTimeOffset ConvertDateTimeOffsetToString(DateTimeOffset dateTimeOffset, string format);
    }
}
