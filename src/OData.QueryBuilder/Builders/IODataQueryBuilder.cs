using OData.QueryBuilder.Conventions.Options;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Builders
{
    public interface IODataQueryBuilder<TResource>
    {
        IODataOption<TEntity> For<TEntity>(Expression<Func<TResource, object>> entityResource);
    }
}