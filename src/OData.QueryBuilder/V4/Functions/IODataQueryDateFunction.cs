using System;

namespace OData.QueryBuilder.V4.Functions
{
    public interface IODataQueryDateFunction
    {
        /// <summary>
        /// http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_date
        /// </summary>
        DateTime Date(DateTimeOffset value);
    }
}
