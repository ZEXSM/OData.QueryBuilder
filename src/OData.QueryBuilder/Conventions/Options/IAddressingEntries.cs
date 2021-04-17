namespace OData.QueryBuilder.Conventions.Options
{
    public interface IAddressingEntries<TEntity>
    {
        IAddressingEntriesKey<TEntity> ByKey(int key);

        IAddressingEntriesKey<TEntity> ByKey(string key);

        IAddressingEntriesCollection<TEntity> ByList();
    }
}
