using OData.QueryBuilder.Conventions.Options.Nested;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Builders
{
    public interface IODataQueryExpandNestedBuilder<TEntity>
    {
        IODataOptionNested<TExpandNestedEntity> For<TExpandNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand);
    }
}
