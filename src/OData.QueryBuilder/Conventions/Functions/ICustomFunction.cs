using System;
using System.Collections.Generic;

namespace OData.QueryBuilder.Conventions.Functions
{
    /// <summary>
    /// Custom functions
    /// </summary>
    public interface ICustomFunction
    {
        /// <summary>
        /// Convert enum to string
        /// </summary>
        T ConvertEnumToString<T>(T value) where T : Enum;

        /// <summary>
        /// Convert datetime to string
        /// </summary>
        DateTime ConvertDateTimeToString(DateTime value, string format);

        /// <summary>
        /// Convert datetimeoffset to string
        /// </summary>
        DateTimeOffset ConvertDateTimeOffsetToString(DateTimeOffset value, string format);

        /// <summary>
        /// Replace characters
        /// </summary>
        string ReplaceCharacters(string value, IDictionary<string, string> keyValuePairs);

        /// <summary>
        /// Replace characters
        /// </summary>
        IEnumerable<string> ReplaceCharacters(IEnumerable<string> values, IDictionary<string, string> keyValuePairs);

        /// <summary>
        /// Dynamic property
        /// </summary>
        /// <param name="propertyPath">The path to the property or field.</param>
        /// <typeparam name="T">The type of the property.</typeparam>
        T Property<T>(string propertyPath);
    }
}
