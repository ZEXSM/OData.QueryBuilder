using System;
using System.Collections.Generic;

namespace OData.QueryBuilder.Conventions.Operators
{
    public interface IODataOperator
    {
        bool In<T>(T columnName, IEnumerable<T> values);

        bool All<T>(IEnumerable<T> columnName, Func<T, bool> func);

        bool Any<T>(IEnumerable<T> columnName, Func<T, bool> func);
    }
}
