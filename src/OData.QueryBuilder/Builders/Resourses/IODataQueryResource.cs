using OData.QueryBuilder.Conventions.Options;

namespace OData.QueryBuilder.Builders.Resourses
{
    public interface IODataQueryResource<TEntity>
    {
        IODataOptionKey<TEntity> ByKey(int key);

        IODataOptionKey<TEntity> ByKey(string key);

        IODataOptionList<TEntity> ByList();
    }
}
