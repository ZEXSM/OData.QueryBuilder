using OData.QueryBuilder.Conventions.AddressingEntities.Options;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query
{
    public interface IODataQueryKey<TEntity> : IODataOptionKey<IODataQueryKey<TEntity>, TEntity>, IODataQuery
    {
        public IAddressingEntries<TResource> For<TResource>(Expression<Func<TEntity, object>> resource);
    }
}
