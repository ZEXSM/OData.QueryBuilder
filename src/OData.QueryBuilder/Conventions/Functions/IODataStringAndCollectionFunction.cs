namespace OData.QueryBuilder.Conventions.Functions
{
    /// <summary>
    /// http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_StringandCollectionFunctions
    /// </summary>
    public interface IODataStringAndCollectionFunction
    {
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
    }
}
