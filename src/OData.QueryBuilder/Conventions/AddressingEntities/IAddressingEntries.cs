using OData.QueryBuilder.Conventions.AddressingEntities.Query;
using System;

namespace OData.QueryBuilder.Conventions.AddressingEntities
{
    public interface IAddressingEntries<TEntity>
    {
        IODataQueryKey<TEntity> ByKey(params int[] keys);

        IODataQueryKey<TEntity> ByKey(params string[] keys);

        IODataQueryKey<TEntity> ByKey(params Guid[] keys);

        IODataQueryCollection<TEntity> ByList();
    }
}
