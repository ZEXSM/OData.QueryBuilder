using OData.QueryBuilder.Conventions.AddressingEntities.Options;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Expand
{
    public interface IODataQueryExpand<TEntity> : IODataOptionExpand<IODataQueryExpand<TEntity>, TEntity>
    {
    }
}
