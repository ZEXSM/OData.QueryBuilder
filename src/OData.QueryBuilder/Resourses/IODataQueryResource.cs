using OData.QueryBuilder.Options;

namespace OData.QueryBuilder.Resourses
{
    public interface IODataQueryResource<TEntity>
    {
        IODataQueryOptionKey<TEntity> ByKey(int key);

        IODataQueryOptionKey<TEntity> ByKey(string key);

        IODataQueryOptionList<TEntity> ByList();
    }
}
