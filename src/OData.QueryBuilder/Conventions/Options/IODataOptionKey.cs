using OData.QueryBuilder.Builders;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.Options
{
    public interface IODataOptionKey<TEntity> : IODataQuery
    {
        IODataOptionKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IODataOptionKey<TEntity> Expand(Action<IODataQueryExpandNestedBuilder<TEntity>> entityExpandNested);

        IODataOptionKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);
    }
}
