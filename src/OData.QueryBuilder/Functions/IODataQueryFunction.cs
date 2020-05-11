using System;
using System.Collections.Generic;

namespace OData.QueryBuilder.Functions
{
    public interface IODataQueryFunction
    {
        DateTime Date(DateTimeOffset dateTimeOffset);

        string Substringof();

        IEnumerable<string> In(IEnumerable<string> enumerable);

        IEnumerable<int> In(IEnumerable<int> enumerable);

        IEnumerable<int?> In(IEnumerable<int?> enumerable);
    }
}
