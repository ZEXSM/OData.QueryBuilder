using OData.QueryBuilder.Conventions.AddressingEntities.Expand;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Resources
{
    public interface IODataExpandResource<TEntity>
    {
        IODataQueryExpand<TExpandNestedEntity> For<TExpandNestedEntity>(Expression<Func<TEntity, object>> nestedExpand);
    }
}
