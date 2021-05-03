using OData.QueryBuilder.Conventions.AddressingEntities.Options;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query.Expand
{
    public interface IODataQueryExpand<TEntity> : IODataOptionCollection<IODataQueryExpand<TEntity>, TEntity>
    {
    }
}
