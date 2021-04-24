using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Resources
{
    internal interface IODataResource<TResource>
    {
        IAddressingEntries<TEntity> For<TEntity>(Expression<Func<TResource, object>> resource);
    }
}
