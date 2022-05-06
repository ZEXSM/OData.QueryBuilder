using System;

namespace OData.QueryBuilder.Conventions.Functions
{
    /// <summary>
    /// https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_DateandTimeFunctions
    /// </summary>
    public interface IODataDateTimeFunction
    {
        /// <summary>
        /// http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_date
        /// </summary>
        DateTime Date(DateTimeOffset value);
    }
}
