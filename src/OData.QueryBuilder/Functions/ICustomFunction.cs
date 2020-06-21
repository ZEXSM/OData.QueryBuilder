using System;

namespace OData.QueryBuilder.Functions
{
    public interface ICustomFunction
    {
        T ConvertEnumToString<T>(T type) where T : Enum;

        DateTime ConvertDateTimeToString(DateTime dateTime, string format);

        DateTime? ConvertDateTimeToString(DateTime? dateTime, string format);

        DateTimeOffset ConvertDateTimeOffsetToString(DateTimeOffset dateTimeOffset, string format);

        DateTimeOffset? ConvertDateTimeOffsetToString(DateTimeOffset? dateTimeOffset, string format);
    }
}
