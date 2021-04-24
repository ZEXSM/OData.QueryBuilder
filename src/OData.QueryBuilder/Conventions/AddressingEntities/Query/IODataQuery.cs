using System;
using System.Collections.Generic;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query
{
    public interface IODataQuery
    {
        Uri ToUri();

        string ToRelativeUri();

        IDictionary<string, string> ToDictionary();
    }
}
