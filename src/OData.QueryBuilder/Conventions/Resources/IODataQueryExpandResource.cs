using OData.QueryBuilder.Conventions.Options.Expand;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.Resources
{
    public interface IODataQueryExpandResource<TEntity>
    {
        IODataOptionExpand<TExpandNestedEntity> For<TExpandNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand);
    }
}
