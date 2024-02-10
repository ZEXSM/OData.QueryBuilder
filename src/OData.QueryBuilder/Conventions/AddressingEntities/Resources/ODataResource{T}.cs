using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Resources
{
    internal class ODataResource<TResource> : IODataResource<TResource>
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

        public IAddressingEntries<TEntity> For<TEntity>(Expression<Func<TResource, object>> resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource), "Resource name is null");
            }

            var query = new ODataResourceExpressionVisitor().ToString(resource);

            _queryBuilder.Append(query);

            return new AddressingEntries<TEntity>(_queryBuilder, _odataQueryBuilderOptions);
        }
    }
}
