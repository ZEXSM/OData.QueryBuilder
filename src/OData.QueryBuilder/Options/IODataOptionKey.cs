using OData.QueryBuilder.Builders;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Options
{
    public interface IODataOptionKey<TEntity> : IODataQuery
    {
        IODataOptionKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IODataOptionKey<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested);

        IODataOptionKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);
    }
}
