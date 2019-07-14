using OData.QueryBuilder.Parameters.Nested;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Builders.Nested
{
    public interface IODataQueryNestedBuilder<TEntity>
    {
        IODataQueryNestedParameter<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand);
    }
}
