namespace OData.QueryBuilder.Conventions.AddressingEntities
{
    public interface IAddressingEntries<TEntity>
    {
        IODataQueryKey<TEntity> ByKey(int key);

        IODataQueryKey<TEntity> ByKey(string key);

        IODataQueryCollection<TEntity> ByList();
    }
}
