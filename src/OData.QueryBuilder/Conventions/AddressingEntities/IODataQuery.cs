using System;
using System.Collections.Generic;

namespace OData.QueryBuilder.Conventions.AddressingEntities
{
    public interface IODataQuery
    {
        Uri ToUri();

        Dictionary<string, string> ToDictionary();
    }
}
