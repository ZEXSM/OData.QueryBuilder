using OData.QueryBuilder.Conventions.AddressingEntities.Query.Expand;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Resources.Expand
{
    public interface IODataExpandResource<TEntity>
    {
        IODataQueryExpand<TExpandNestedEntity> For<TExpandNestedEntity>(Expression<Func<TEntity, object>> expandNested);
    }
}
