using OData.QueryBuilder.Conventions.Options.Nested;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Resources
{
    public interface IODataQueryExpandNestedResource<TEntity>
    {
        IODataOptionNested<TExpandNestedEntity> For<TExpandNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand);
    }
}
