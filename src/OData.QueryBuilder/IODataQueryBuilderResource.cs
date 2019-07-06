namespace OData.QueryBuilder
{
    public interface IODataQueryBuilderResource<TEntity>
    {
        IODataQueryBuilderKey<TEntity> ByKey(int key);

        IODataQueryBuilderList<TEntity> ByList();
    }
}
