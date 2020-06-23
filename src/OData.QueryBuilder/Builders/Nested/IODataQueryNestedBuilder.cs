using OData.QueryBuilder.Options.Nested;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Builders.Nested
{
    public interface IODataQueryNestedBuilder<TEntity>
    {
        IODataQueryOptionNested<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand);
    }
}
