using System;

namespace OData.QueryBuilder.Functions
{
    public interface IODataFunction
    {
        string Date(DateTime date);

        bool All();

        bool Any();
    }
}
