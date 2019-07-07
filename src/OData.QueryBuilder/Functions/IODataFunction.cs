using System;

namespace OData.QueryBuilder.Functions
{
    public interface IODataFunction
    {
        string Date(DateTime date);

        string All();

        string Any();
    }
}
