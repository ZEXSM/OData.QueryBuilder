using OData.QueryBuilder.Builders.Nested;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Options
{
    public interface IODataQueryOptionKey<TEntity> : IODataQuery
    {
        IODataQueryOptionKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IODataQueryOptionKey<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested);

        IODataQueryOptionKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);
    }
}
