using System;
using System.Collections.Generic;

namespace OData.QueryBuilder.Operators
{
    public interface IODataQueryOperator
    {
        bool In<T>(T columnName, IEnumerable<T> values);

        bool All<T>(T[] columnName, Func<T, bool> func);

        bool Any<T>(T[] columnName, Func<T, bool> func);
    }
}
