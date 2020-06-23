namespace OData.QueryBuilder.Conventions.Options
{
    public interface IODataOption<TEntity>
    {
        IODataOptionKey<TEntity> ByKey(int key);

        IODataOptionKey<TEntity> ByKey(string key);

        IODataOptionList<TEntity> ByList();
    }
}
