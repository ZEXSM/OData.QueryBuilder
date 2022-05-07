using System;

namespace OData.QueryBuilder.Conventions.Functions
{
    /// <summary>
    /// http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_StringandCollectionFunctions
    /// </summary>
    public interface IODataStringAndCollectionFunction
    {
        /// <summary>
        /// 
        /// </summary>
        [Obsolete("Use Contains OData Version 4.0 Protocol function if possible")]
        bool SubstringOf(string value, string columnName);

        /// <summary>
        /// http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_contains
        /// </summary>
        bool Contains(string columnName, string value);

        /// <summary>
        /// http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_toupper
        /// </summary>
        string ToUpper(string columnName);

        /// <summary>
        /// http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_tolower
        /// </summary>
        string ToLower(string columnName);

        /// <summary>
        /// http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_concat
        /// </summary>
        string Concat(string s1, string s2);

        /// <summary>
        /// http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_indexof
        /// </summary>
        int IndexOf(string columnName, string value);

        /// <summary>
        /// http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_startswith
        /// </summary>
        bool StartsWith(string columnName, string value);

        /// <summary>
        /// https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_length
        /// </summary>
        int Length(string columnName);
    }
}
