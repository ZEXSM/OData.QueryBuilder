using System;
using System.Collections.Generic;

namespace OData.QueryBuilder.Parameters
{
    public interface IODataQueryParameter
    {
        Uri ToUri();

        Dictionary<string, string> ToDictionary();
    }
}
