using OData.QueryBuilder.Builders.Nested;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Parameters
{
    public interface IODataQueryParameterKey<TEntity> : IODataQueryParameter
    {
        IODataQueryParameterKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IODataQueryParameterKey<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested);

        IODataQueryParameterKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);
    }
}
