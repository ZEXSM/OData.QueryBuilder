namespace OData.QueryBuilder.Functions
{
    public interface IODataQueryStringFunction
    {
        bool SubstringOf(string substring, string columnName);

        /// <summary>
        /// http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_toupper
        /// </summary>
        string ToUpper(string columnName);
    }
}
