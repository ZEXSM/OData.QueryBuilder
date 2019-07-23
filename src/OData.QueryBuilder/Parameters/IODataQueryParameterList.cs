using OData.QueryBuilder.Builders.Nested;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Parameters
{
    public interface IODataQueryParameterList<TEntity> : IODataQueryParameter
    {
        IODataQueryParameterList<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter);

        IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IODataQueryParameterList<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested);

        IODataQueryParameterList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);

        IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy);

        IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending);

        IODataQueryParameterList<TEntity> Top(int number);

        IODataQueryParameterList<TEntity> Skip(int number);

        IODataQueryParameterList<TEntity> Count();
    }
}