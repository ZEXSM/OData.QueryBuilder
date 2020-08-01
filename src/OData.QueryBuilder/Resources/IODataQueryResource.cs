using OData.QueryBuilder.Conventions.Options;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Resources
{
    public interface IODataQueryResource<TResource>
    {
        IODataOption<TEntity> For<TEntity>(Expression<Func<TResource, object>> entityResource);
    }
}
