using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder
{
    public interface IODataQueryBuilder<TResource>
    {
        IODataQueryBuilderResource<TEntity> ForResource<TEntity>(Expression<Func<TResource, object>> resource);
    }
}