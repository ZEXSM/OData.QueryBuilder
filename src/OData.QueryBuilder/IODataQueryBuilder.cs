using OData.QueryBuilder.Resourses;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder
{
    public interface IODataQueryBuilder<TResource>
    {
        IODataQueryResource<TEntity> ForResource<TEntity>(Expression<Func<TResource, object>> resource);
    }
}