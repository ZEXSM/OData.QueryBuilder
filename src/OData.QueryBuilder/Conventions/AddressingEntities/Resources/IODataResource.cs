using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Resources
{
    public interface IODataResource<TResource>
    {
        IAddressingEntries<TEntity> For<TEntity>(Expression<Func<TResource, object>> resource);
    }
}
