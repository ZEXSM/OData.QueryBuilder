using System;

namespace OData.QueryBuilder.Parameters
{
    public interface IODataQueryParameter
    {
        Uri ToUri();
    }
}
