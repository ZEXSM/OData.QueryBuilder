using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Options;
using System;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Resources
{
    internal class ODataResource : IODataResource
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly QBuilder _queryBuilder;

        public ODataResource(
            QBuilder queryBuilder,
            ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
            _queryBuilder = queryBuilder;
        }

        public IAddressingEntries<TEntity> For<TEntity>(string resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource), "Resource name is null");
            }

            if (resource != string.Empty)
            {
                _queryBuilder.Append(resource);
            }

            return new AddressingEntries<TEntity>(_queryBuilder, _odataQueryBuilderOptions);
        }
    }
}
