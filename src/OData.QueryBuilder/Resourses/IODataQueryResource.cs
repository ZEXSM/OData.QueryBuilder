using OData.QueryBuilder.Parameters;

namespace OData.QueryBuilder.Resourses
{
    public interface IODataQueryResource<TEntity>
    {
        IODataQueryParameterKey<TEntity> ByKey(int key);

        IODataQueryParameterKey<TEntity> ByKey(string key);

        IODataQueryParameterList<TEntity> ByList();
    }
}
