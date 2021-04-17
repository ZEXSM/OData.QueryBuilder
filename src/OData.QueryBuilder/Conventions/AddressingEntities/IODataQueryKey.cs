using OData.QueryBuilder.Conventions.AddressingEntities.Options;

namespace OData.QueryBuilder.Conventions.AddressingEntities
{
    public interface IODataQueryKey<TEntity> : IODataOptionKey<IODataQueryKey<TEntity>, TEntity>, IODataQuery
    {
    }
}
