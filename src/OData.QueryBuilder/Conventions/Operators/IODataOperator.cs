using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.Operators
{
    public interface IODataOperator
    {
        bool In<T>(T columnName, IEnumerable<T> values);

        bool All<T>(IEnumerable<T> columnName, Expression<Func<T, bool>> func);

        bool Any<T>(IEnumerable<T> columnName);
        
        bool Any<T>(IEnumerable<T> columnName, Expression<Func<T, bool>> func);
    }
}
