using System;
using System.Collections.Generic;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query
{
    public interface IODataQuery
    {
        Uri ToUri();

        IDictionary<string, string> ToDictionary();
    }
}
