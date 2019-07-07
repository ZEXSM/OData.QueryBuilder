using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Parameters
{
    public interface IODataQueryParameterKey<TEntity>
    {
        IODataQueryParameterKey<TEntity> Expand(Expression<Func<IODataQueryNestedParameter<TEntity>, TEntity, object>> entityExpand);

        IODataQueryParameterKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IODataQueryParameterKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);

        Uri ToUri();
    }
}
