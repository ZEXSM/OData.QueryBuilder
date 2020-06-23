using OData.QueryBuilder.Conventions.Options.Nested;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Builders
{
    public interface IODataQueryNestedBuilder<TEntity>
    {
        IODataOptionNested<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand);
    }
}
