using System;
using System.Collections.Generic;

namespace OData.QueryBuilder.Functions
{
    public interface IODataQueryFunction
    {
        DateTime Date(DateTimeOffset dateTimeOffset);

        string Substringof();

        bool In(IEnumerable<string> enumerable);

        bool In(IEnumerable<int> enumerable);
    }
}
