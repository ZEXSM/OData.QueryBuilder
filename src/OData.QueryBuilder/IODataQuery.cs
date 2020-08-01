using System;
using System.Collections.Generic;

namespace OData.QueryBuilder
{
    public interface IODataQuery
    {
        Uri ToUri();

        Dictionary<string, string> ToDictionary();
    }
}
