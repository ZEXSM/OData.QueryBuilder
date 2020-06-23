using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Functions;
using OData.QueryBuilder.Operators;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Options
{
    public interface IODataQueryOptionList<TEntity> : IODataQuery
    {
        IODataQueryOptionList<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter);

        IODataQueryOptionList<TEntity> Filter(Expression<Func<TEntity, IODataQueryFunction, bool>> entityFilter);

        IODataQueryOptionList<TEntity> Filter(Expression<Func<TEntity, IODataQueryFunction, IODataQueryOperator, bool>> entityFilter);

        IODataQueryOptionList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IODataQueryOptionList<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested);

        IODataQueryOptionList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);

        IODataQueryOptionList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy);

        IODataQueryOptionList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending);

        IODataQueryOptionList<TEntity> Top(int number);

        IODataQueryOptionList<TEntity> Skip(int number);

        IODataQueryOptionList<TEntity> Count(bool value = true);
    }
}