using OData.QueryBuilder.Resourses;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Builders
{
    public interface IODataQueryBuilder<TResource>
    {
        IODataQueryResource<TEntity> For<TEntity>(Expression<Func<TResource, object>> entityResource);
    }
}