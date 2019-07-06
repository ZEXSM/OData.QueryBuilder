using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder
{
    public interface IODataQueryBuilderKey<TEntity>
    {
        IODataQueryBuilderKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IODataQueryBuilderKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);

        Uri ToUri();
    }
}
