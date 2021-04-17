using OData.QueryBuilder.Conventions.AddressingEntities.Options;

namespace OData.QueryBuilder.Conventions.AddressingEntities
{
    public interface IODataQueryCollection<TEntity> : IODataOptionCollection<IODataQueryCollection<TEntity>, TEntity>, IODataQuery
    {
    }
}