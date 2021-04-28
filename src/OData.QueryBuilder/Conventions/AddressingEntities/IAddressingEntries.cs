using OData.QueryBuilder.Conventions.AddressingEntities.Query;
using System;

namespace OData.QueryBuilder.Conventions.AddressingEntities
{
    public interface IAddressingEntries<TEntity>
    {
        IODataQueryKey<TEntity> ByKey(int key);

        IODataQueryKey<TEntity> ByKey(string key);

        IODataQueryKey<TEntity> ByKey(Guid key);

        IODataQueryCollection<TEntity> ByList();
    }
}
