namespace OData.QueryBuilder.Conventions.AddressingEntities.Resources
{
    internal interface IODataResource
    {
        IAddressingEntries<TEntity> For<TEntity>(string resource);
    }
}
