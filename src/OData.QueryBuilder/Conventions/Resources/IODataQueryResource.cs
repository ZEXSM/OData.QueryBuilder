using OData.QueryBuilder.Conventions.Options;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.Resources
{
    public interface IODataQueryResource<TResource>
    {
        IAddressingEntries<TEntity> For<TEntity>(Expression<Func<TResource, object>> entityResource);
    }
}
