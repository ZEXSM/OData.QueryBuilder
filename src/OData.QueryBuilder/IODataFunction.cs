using System;

namespace OData.QueryBuilder
{
    public interface IODataFunction
    {
        string Date(DateTime date);

        string All();

        string Any();
    }
}
