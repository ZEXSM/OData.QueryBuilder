using System;
using System.Collections.Generic;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query
{
    public interface IODataQuery
    {
        Uri ToUri(UriKind uriKind = UriKind.RelativeOrAbsolute);

        IDictionary<string, string> ToDictionary();
    }
}
